<template>
  <div class="my-orders-page ">
    <div class="container">
      <nav aria-label="breadcrumb-cart">
        <ol class="breadcrumb">
          <li class="breadcrumb-item">
            <router-link to="/">Trang chủ</router-link>
          </li>
          <li class="breadcrumb-item">
            <router-link to="/don-hang-cua-toi">Đơn hàng của tôi</router-link>
          </li>
        </ol>
      </nav>
      <div class="orders-header">
        <div class="header-left">
          <i class="bi bi-box-seam"></i>
          <h1 class="page-title">ĐƠN HÀNG CỦA TÔI</h1>
        </div>
        <button @click="loadOrders" class="btn-refresh" :disabled="isLoading">
          <i class="bi bi-arrow-clockwise" :class="{ 'spinning': isLoading }"></i>
          Làm mới
        </button>
      </div>
      <div v-if="isLoading" class="loading-state">
        <div class="spinner-border text-primary" role="status">
          <span class="visually-hidden">Loading...</span>
        </div>
        <p>Đang tải đơn hàng...</p>
      </div>
      <div v-else-if="orders.length === 0" class="empty-state">
        <div class="empty-icon">
          <i class="bi bi-inbox"></i>
        </div>
        <h3>Chưa có đơn hàng nào</h3>
        <p>Bạn chưa có đơn hàng nào. Hãy bắt đầu mua sắm ngay!</p>
        <router-link to="/san-pham" class="btn-shop-now">
          <i class="bi bi-bag-check"></i> Mua sắm ngay
        </router-link>
      </div>
      <div v-else class="orders-list">
        <OrderCard
          v-for="order in orders"
          :key="order.id"
          :order="order"
          @view-detail="viewOrderDetail"
          @cancel-order="handleCancelOrder"
        />
      </div>
    </div>
    <OrderDetailModal
      :is-open="isModalOpen"
      :order-id="selectedOrderId"
      @close="closeModal"
      @order-updated="loadOrders"
    />
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import Swal from 'sweetalert2'
import OrderService from '@/services/OrderService'
import OrderCard from '@/components/order/OrderCard.vue'
import OrderDetailModal from '@/components/order/OrderDetailModal.vue'

const router = useRouter()
const orders = ref([])
const isLoading = ref(false)
const isModalOpen = ref(false)
const selectedOrderId = ref(null)

const loadOrders = async () => {
  isLoading.value = true
  try {
    const data = await OrderService.getMyOrders()
    orders.value = data
  } catch (error) {
    console.error('Error loading orders:', error)
    await Swal.fire({
      icon: 'error',
      title: 'Lỗi',
      text: error.message || 'Không thể tải danh sách đơn hàng',
      confirmButtonText: 'OK'
    })
  } finally {
    isLoading.value = false
  }
}

const viewOrderDetail = (orderId) => {
  selectedOrderId.value = orderId
  isModalOpen.value = true
}

const closeModal = () => {
  isModalOpen.value = false
  selectedOrderId.value = null
}

const handleCancelOrder = async (orderId, orderCode) => {
  const result = await Swal.fire({
    icon: 'warning',
    title: 'Xác nhận hủy đơn hàng',
    html: `
      <p>Bạn có chắc chắn muốn hủy đơn hàng</p>
      <p style="color:#e74c3c; font-weight:700; font-size:1.1rem; margin-top:8px;">
        ${orderCode}
      </p>
      <p style="color:#666; margin-top:12px;">Hành động này không thể hoàn tác.</p>
    `,
    showCancelButton: true,
    confirmButtonText: 'Hủy đơn hàng',
    cancelButtonText: 'Không',
    confirmButtonColor: '#e74c3c',
    cancelButtonColor: '#6c757d'
  })
  if (result.isConfirmed) {
    try {
      await OrderService.cancelOrder(orderId)
      await Swal.fire({
        icon: 'success',
        title: 'Đã hủy đơn hàng',
        text: 'Đơn hàng của bạn đã được hủy thành công.',
        timer: 2000,
        showConfirmButton: false
      })
      await loadOrders()
    } catch (error) {
      console.error('Error canceling order:', error)
      await Swal.fire({
        icon: 'error',
        title: 'Không thể hủy đơn hàng',
        text: error.message || 'Đơn hàng không thể hủy lúc này.',
        confirmButtonText: 'OK'
      })
    }
  }
}

onMounted(() => {
  loadOrders()
})
</script>

<style scoped>
.my-orders-page {
  padding: 30px;
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

.orders-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  background: white;
  padding: 0.8rem 1.2rem;
}

.header-left {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.header-left i {
  font-size: 2rem;
  color: #3b82f6;
}

.page-title {
  font-weight: 700;
  font-size: 1.2rem;
  color: #1f2937;
  margin: 0;
  letter-spacing: 0.5px;
}

.btn-refresh {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.2rem 0.8rem;
  background: white;
  color: #3b82f6;
  border: 2px solid #3b82f6;
  border-radius: 8px;
  font-weight: 600;
  font-size: 0.95rem;
  cursor: pointer;
  transition: all 0.3s ease;
}

.btn-refresh:hover:not(:disabled) {
  background: #3b82f6;
  color: white;
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(59, 130, 246, 0.3);
}

.btn-refresh:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn-refresh i {
  font-size: 1.1rem;
}

.spinning {
  animation: spin 1s linear infinite;
}

@keyframes spin {
  from { transform: rotate(0deg); }
  to { transform: rotate(360deg); }
}

.loading-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 4rem 2rem;
  background: white;
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
}

.loading-state p {
  margin-top: 1rem;
  color: #6b7280;
  font-size: 1rem;
}

.empty-state {
  text-align: center;
  padding: 4rem 2rem;
  background: white;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
}

.empty-icon {
  font-size: 5rem;
  color: #d1d5db;
  margin-bottom: 1.5rem;
  animation: float 3s ease-in-out infinite;
}

@keyframes float {
  0%, 100% { transform: translateY(0); }
  50% { transform: translateY(-15px); }
}

.empty-state h3 {
  font-size: 1.5rem;
  font-weight: 700;
  color: #1f2937;
  margin-bottom: 0.75rem;
}

.empty-state p {
  font-size: 1rem;
  color: #6b7280;
  margin-bottom: 2rem;
}

.btn-shop-now {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.875rem 2rem;
  background: var(--gradient);
  color: white;
  border: none;
  border-radius: 50px;
  font-weight: 600;
  font-size: 1rem;
  text-decoration: none;
  transition: all 0.3s ease;
  box-shadow: 0 4px 15px rgba(102, 126, 234, 0.3);
}

.btn-shop-now:hover {
  transform: translateY(-3px);
  box-shadow: 0 6px 20px rgba(102, 126, 234, 0.4);
}

.orders-list {
  display: flex;
  flex-direction: column;
}

@media (max-width: 768px) {
  .my-orders-page {
    padding: 1rem 0;
  }

  .orders-header {
    flex-direction: column;
    gap: 1rem;
    padding: 1.25rem 1.5rem;
  }

  .header-left {
    width: 100%;
    justify-content: center;
  }

  .header-left i {
    font-size: 1.5rem;
  }

  .page-title {
    font-size: 1.4rem;
  }

  .btn-refresh {
    width: 100%;
    justify-content: center;
  }
}

@media (max-width: 576px) {
  .container {
    padding: 0 0.75rem;
  }

  .orders-header {
    padding: 1rem;
  }

  .page-title {
    font-size: 1.25rem;
  }

  .empty-icon {
    font-size: 4rem;
  }

  .empty-state h3 {
    font-size: 1.25rem;
  }

  .empty-state p {
    font-size: 0.9rem;
  }
}
</style>