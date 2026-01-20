<template>
  <div class="wishlist-page">
    <div class="container">
      <nav aria-label="breadcrumb-cart">
        <ol class="breadcrumb">
          <li class="breadcrumb-item">
            <router-link to="/">Trang chủ</router-link>
          </li>
          <li class="breadcrumb-item">
            <router-link to="/san-pham-yeu-thich">Sản phẩm yêu thích</router-link>
          </li>
        </ol>
      </nav>
      <div class="wishlist-header">
        <div class="header-left">
          <i class="bi bi-heart-fill"></i>
          <div>
            <h1 class="page-title">Danh sách yêu thích</h1>
            <p class="subtitle">{{ wishlistData.tongSanPham }} sản phẩm</p>
          </div>
        </div>
        <button
          v-if="wishlistData.tongSanPham > 0"
          class="btn-clear-all"
          @click="clearAllConfirm"
        >
          <i class="bi bi-trash"></i>
          Xóa tất cả
        </button>
      </div>
      <div v-if="loading" class="loading-state">
        <div class="spinner-border text-primary"></div>
        <p>Đang tải danh sách yêu thích...</p>
      </div>
      <div v-else-if="wishlistData.tongSanPham === 0" class="empty-state">
        <i class="bi bi-heart empty-icon"></i>
        <h3>Danh sách yêu thích trống</h3>
        <p>Bạn chưa thêm sản phẩm nào vào danh sách yêu thích</p>
        <router-link to="/san-pham" class="btn-shop-now">
          <i class="bi bi-shop"></i>
          Khám phá sản phẩm
        </router-link>
      </div>
      <div v-else class="wishlist-list">
        <div class="wishlist-table-header">
          <div class="th-image">Sản phẩm</div>
          <div class="th-info">Thông tin</div>
          <div class="th-price">Giá</div>
          <div class="th-stock">Trạng thái</div>
          <div class="th-action">Hành động</div>
        </div>
        <WishlistItem
          v-for="item in wishlistData.sanPham"
          :key="item.maSanPham"
          :item="item"
          @removed="handleItemRemoved"
        />
        <div class="back-shopping">
          <router-link to="/san-pham" class="btn-back">
            <i class="bi bi-arrow-left"></i>
            Tiếp tục mua sắm
          </router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import Swal from 'sweetalert2'
import WishlistItem from '@/components/wishlist/WishlistItem.vue'
import WishlistService from '@/services/WishlistService'

const loading = ref(false)
const wishlistData = ref({
  tongSanPham: 0,
  sanPham: []
})

const fetchWishlist = async () => {
  loading.value = true
  try {
    const data = await WishlistService.getWishlist()
    wishlistData.value = data
  } catch (error) {
      Swal.fire({
        icon: 'error',
        title: 'Lỗi!',
        text: 'Không thể tải danh sách yêu thích',
        timer: 2000,
        showConfirmButton: false
      })
  } finally {
    loading.value = false
  }
}

const handleItemRemoved = (maSanPham) => {
  wishlistData.value.sanPham = wishlistData.value.sanPham.filter(item => item.maSanPham !== maSanPham)
  wishlistData.value.tongSanPham--
}

const clearAllConfirm = async () => {
  const result = await Swal.fire({
    title: 'Xác nhận xóa tất cả?',
    text: 'Bạn có chắc muốn xóa tất cả sản phẩm yêu thích?',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#d33',
    cancelButtonColor: '#3085d6',
    confirmButtonText: 'Xóa tất cả',
    cancelButtonText: 'Hủy'
  })
  if (result.isConfirmed) {
    await clearAll()
  }
}

const clearAll = async () => {
  const items = [...wishlistData.value.sanPham]
  Swal.fire({
    title: 'Đang xóa...',
    text: 'Vui lòng đợi',
    allowOutsideClick: false,
    didOpen: () => {
      Swal.showLoading()
    }
  })
  try {
    for (const item of items) {
      await WishlistService.removeFromWishlist(item.maSanPham)
    }
    wishlistData.value = { tongSanPham: 0, sanPham: [] }
    Swal.fire({
      icon: 'success',
      title: 'Thành công!',
      text: 'Đã xóa tất cả sản phẩm yêu thích',
      timer: 2000,
      showConfirmButton: false
    })
  } catch (error) {
    Swal.fire({
      icon: 'error',
      title: 'Lỗi!',
      text: 'Có lỗi xảy ra khi xóa',
      timer: 2000,
      showConfirmButton: false
    })
  }
}

onMounted(() => {
  fetchWishlist()
})
</script>
  
<style scoped>
.wishlist-page {
  padding: 30px 0;
  background: #f5f5f5;
}

.container {
  max-width: 1200px;
}

.breadcrumb {
  background: transparent;
  padding: 0.5rem 0;
}

.breadcrumb-item a {
  color: var(--primary-color);
  font-size: 0.95rem;
}

.wishlist-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  background: white;
  padding: 1rem 1.5rem;
}

.header-left {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.header-left i {
  font-size: 2rem;
  color: #ef4444;
}

.page-title {
  margin: 0;
  font-size: 1.3rem;
  font-weight: 700;
  color: #1f2937;
}

.subtitle {
  margin: 0;
  font-size: 0.95rem;
  color: #6b7280;
}

.wishlist-table-header {
  display: grid;
  grid-template-columns: 110px 1.5fr 1fr 0.8fr 0.8fr;
  padding: 1.25rem 1.5rem;
  background: #f8f9fa;
  border: 1px solid #e5e7eb;
  font-size: 0.85rem;
  font-weight: 700;
  color: #374151;
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

.wishlist-table-header > div {
  display: flex;
  align-items: center;
}

.th-image {
  justify-content: center;
}

.th-info {
  justify-content: center;
}

.th-price {
  justify-content: center;
}

.th-stock {
  justify-content: center;
}

.th-action {
  justify-content: center;
}

.btn-clear-all {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.4rem 1.1rem;
  border-radius: 8px;
  background: white;
  border: 2px solid #ef4444;
  color: #ef4444;
  font-weight: 600;
  transition: all 0.3s ease;
}

.btn-clear-all:hover {
  background: #ef4444;
  color: white;
  box-shadow: 0 4px 12px rgba(239, 68, 68, 0.3);
}

.loading-state,
.empty-state {
  background: white;
  padding: 4rem 2rem;
  text-align: center;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
}

.empty-icon {
  font-size: 5rem;
  color: #d1d5db;
  margin-bottom: 1.5rem;
}

.empty-state h3 {
  font-size: 1.5rem;
  font-weight: 700;
}

.empty-state p {
  color: #6b7280;
  margin-bottom: 2rem;
}

.btn-shop-now {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.85rem 2rem;
  background: linear-gradient(135deg, #ef4444, #f97316);
  color: white;
  border-radius: 50px;
  font-weight: 600;
  text-decoration: none;
}

.wishlist-list {
  display: flex;
  flex-direction: column;
}

.back-shopping {
  text-align: center;
  margin-top: 2rem;
}

.btn-back {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.5rem 1.2rem;
  border-radius: 8px;
  border: 2px solid #3b82f6;
  color: #3b82f6;
  font-weight: 600;
  text-decoration: none;
}

.btn-back:hover {
  background: #3b82f6;
  color: white;
}
</style>