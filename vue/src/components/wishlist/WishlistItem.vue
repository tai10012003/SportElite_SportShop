<template>
  <div class="wishlist-row">
    <div class="td-image">
      <img :src="getImagePath(item)" :alt="item.tenSanPham" />
    </div>
    <div class="td-info">
      <router-link
        :to="`/san-pham/${item.maSanPham}`"
        class="product-name"
      >
        {{ item.tenSanPham }}
      </router-link>
      <div class="category">
        Danh mục: <strong>{{ item.maDanhMuc || 'N/A' }}</strong>
      </div>
      <div class="brand">
        Thương hiệu: <strong>{{ item.maThuongHieu || 'N/A' }}</strong>
      </div>
    </div>
    <div class="td-price">
      <div class="price-new">
        {{ formatPrice(item.giaKhuyenMai || item.gia) }}₫
      </div>
      <div v-if="item.giaKhuyenMai" class="price-old">
        {{ formatPrice(item.gia) }}₫
      </div>
    </div>
    <div class="td-stock">
      <span
        class="stock-badge"
        :class="item.conHang ? 'in-stock' : 'out-stock'"
      >
        {{ item.conHang ? 'Còn hàng' : 'Hết hàng' }}
      </span>
    </div>
    <div class="td-action">
      <button
        class="btn-remove"
        @click="handleRemove"
        :disabled="removing"
      >
        <i class="bi bi-trash"></i>
        Xóa
      </button>
    </div>
  </div>
</template>
  
<script setup>
import { ref } from 'vue'
import WishlistService from '@/services/WishlistService'
import Swal from 'sweetalert2'

const props = defineProps({
  item: {
    type: Object,
    required: true
  }
})

const emit = defineEmits(['removed'])

const removing = ref(false)

const formatPrice = (price) => new Intl.NumberFormat('vi-VN').format(price)

const getImagePath = (item) => {
  if (!item.hinhAnh) return '/images/products/no-image.jpg'
  return `/uploads/products/${item.hinhAnh}`
}

const handleRemove = async () => {
  const result = await Swal.fire({
    title: 'Xác nhận xóa?',
    text: 'Bạn có chắc muốn xóa sản phẩm này khỏi danh sách yêu thích?',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#d33',
    cancelButtonColor: '#3085d6',
    confirmButtonText: 'Xóa',
    cancelButtonText: 'Hủy'
  })
  if (!result.isConfirmed) return
  removing.value = true
  try {
    const response = await WishlistService.removeFromWishlist(props.item.maSanPham)
    if (response.success) {
      Swal.fire({
        icon: 'success',
        title: 'Đã xóa!',
        text: response.message || 'Đã xóa khỏi danh sách yêu thích',
        timer: 2000,
        showConfirmButton: false,
        toast: true,
        position: 'top-end'
      })
      emit('removed', props.item.maSanPham)
    } else {
      Swal.fire({
        icon: 'error',
        title: 'Lỗi!',
        text: response.message || 'Có lỗi xảy ra',
        timer: 2000,
        showConfirmButton: false
      })
    }
  } catch (error) {
    Swal.fire({
      icon: 'error',
      title: 'Lỗi!',
      text: 'Không thể xóa sản phẩm',
      timer: 2000,
      showConfirmButton: false
    })
  } finally {
    removing.value = false
  }
}
</script>

<style scoped>
.wishlist-row {
  display: grid;
  grid-template-columns: 110px 1.5fr 1fr 0.8fr 0.8fr;
  padding: 1.25rem 1.5rem;
  align-items: center;
  background: white;
  border: 1px solid #e5e7eb;
  border-top: none;
}

.wishlist-row > div {
  display: flex;
  align-items: center;
  justify-content: center;
}

.td-image img {
  width: 90px;
  height: 90px;
  object-fit: cover;
  border-radius: 8px;
  border: 1px solid #e5e7eb;
}

.product-name {
  font-size: 1rem;
  font-weight: 600;
  color: #1f2937;
  text-decoration: none;
}

.product-name:hover {
  color: #ef4444;
}

.category .brand {
  font-size: 0.9rem;
  color: #6b7280;
}

.td-info,
.td-price {
  flex-direction: column;
  align-items: center;
}

.price-new {
  font-size: 1.1rem;
  font-weight: 700;
  color: #ef4444;
}

.price-old {
  font-size: 0.9rem;
  color: #9ca3af;
  text-decoration: line-through;
}

.td-stock {
  text-align: center;
}

.stock-badge {
  padding: 0.35rem 0.9rem;
  border-radius: 20px;
  font-size: 0.8rem;
  font-weight: 600;
}

.in-stock {
  background: #d1fae5;
  color: #059669;
}

.out-stock {
  background: #fee2e2;
  color: #dc2626;
}

.td-action {
  display: flex;
  justify-content: center;
}

.btn-remove {
  display: inline-flex;
  align-items: center;
  gap: 0.4rem;
  padding: 0.45rem 1.1rem;
  border-radius: 8px;
  background: white;
  border: 2px solid #ef4444;
  color: #ef4444;
  font-weight: 600;
  transition: all 0.25s ease;
}

.btn-remove:hover:not(:disabled) {
  background: #ef4444;
  color: white;
}

.btn-remove:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

@media (max-width: 992px) {
  .wishlist-table-header {
    display: none;
  }

  .wishlist-row {
    grid-template-columns: 90px 1fr;
    gap: 1rem;
  }

  .td-price,
  .td-stock,
  .td-action {
    grid-column: 2 / -1;
  }

  .td-action {
    justify-content: flex-start;
  }
}
</style>