<template>
  <div class="order-card">
    <div class="order-header">
      <div class="order-code-section">
        <span class="label">Mã đơn hàng:</span>
        <span class="order-code">{{ order.maDonHang }}</span>
      </div>
      <div class="order-meta">
        <div class="order-date">
          <i class="bi bi-calendar3"></i>
          <span>{{ formatDate(order.ngayTao) }}</span>
        </div>
        <span :class="['status-badge', getStatusClass(order.trangThai)]">
          {{ getStatusText(order.trangThai) }}
        </span>
      </div>
    </div>
    <div class="order-body">
      <div class="order-info-left">
        <div class="info-box">
          <span class="info-label">Số lượng sản phẩm</span>
          <div class="payment-method">
            <i class="bi bi-box-seam"></i>
            <span>{{ order.soLuongSanPham }} sản phẩm</span>
          </div>
        </div>
        <div class="info-box">
          <span class="info-label">Tổng tiền</span>
          <span class="info-value total-amount">
            {{ formatPrice(order.tongThanhToan) }}₫
          </span>
        </div>
        <div class="info-box">
          <span class="info-label">Phương thức thanh toán</span>
          <div class="payment-method">
            <i class="bi bi-credit-card"></i>
            <span>{{ getPaymentMethodText(order.phuongThucThanhToan) }}</span>
          </div>
        </div>
      </div>
      <div class="order-actions">
        <button @click="$emit('view-detail', order.id)" class="btn-view-detail">
          <i class="bi bi-eye"></i>
          Xem chi tiết đơn hàng
        </button>
        <button 
          v-if="canCancelOrder"
          @click="$emit('cancel-order', order.id, order.maDonHang)" 
          class="btn-cancel-order"
        >
          <i class="bi bi-x-circle"></i>
          Hủy đơn hàng
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  order: {
    type: Object,
    required: true
  }
})

defineEmits(['view-detail', 'cancel-order'])

const canCancelOrder = computed(() => {
  const cancelableStatuses = [0, 1]
  return cancelableStatuses.includes(props.order.trangThai)
})

const formatPrice = (price) => {
  return new Intl.NumberFormat('vi-VN').format(price)
}

const formatDate = (dateString) => {
  const date = new Date(dateString)
  const day = String(date.getDate()).padStart(2, '0')
  const month = String(date.getMonth() + 1).padStart(2, '0')
  const year = date.getFullYear()
  const hours = String(date.getHours()).padStart(2, '0')
  const minutes = String(date.getMinutes()).padStart(2, '0')
  return `${day}/${month}/${year} ${hours}:${minutes}`
}

const getStatusClass = (status) => {
  const statusMap = {
    0: 'status-pending',
    1: 'status-confirmed',
    2: 'status-shipping',
    3: 'status-completed',
    4: 'status-cancelled'
  }
  return statusMap[status] || 'status-pending'
}

const getStatusText = (status) => {
  const statusMap = {
    0: 'Chờ xác nhận',
    1: 'Đã hoàn thành',
    2: 'Đang giao hàng',
    3: 'Đã hoàn thành',
    4: 'Đã hủy'
  }
  return statusMap[status] || 'Không xác định'
}

const getPaymentMethodText = (method) => {
  const methodMap = {
    'cod': 'COD',
    'vnpay': 'VNPay',
    'momo': 'MoMo'
  }
  return methodMap[method?.toLowerCase()] || method
}
</script>

<style scoped>
.order-card {
  background: white;
  border: 1px solid #e5e7eb;
  overflow: hidden;
  transition: all 0.3s ease;
}

.order-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem 1.5rem;
  background: white;
  border-bottom: 1px solid #e5e7eb;
}

.order-code-section {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.label {
  font-size: 0.9rem;
  color: #6b7280;
  font-weight: 500;
}

.order-code {
  font-size: 1rem;
  font-weight: 700;
  color: #3b82f6;
}

.order-meta {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.order-date {
  display: flex;
  align-items: center;
  gap: 0.4rem;
  color: #6b7280;
  font-size: 0.9rem;
}

.order-date i {
  font-size: 0.95rem;
}

.status-badge {
  padding: 0.35rem 0.85rem;
  border-radius: 20px;
  font-size: 0.8rem;
  font-weight: 600;
}

.status-pending {
  background: #fef3c7;
  color: #d97706;
}

.status-confirmed {
  background: #d1fae5;
  color: #059669;
}

.status-shipping {
  background: #dbeafe;
  color: #2563eb;
}

.status-completed {
  background: #d1fae5;
  color: #059669;
}

.status-cancelled {
  background: #fee2e2;
  color: #dc2626;
}

.order-body {
  display: flex;
  justify-content: space-between;
  align-items: stretch;
  padding: 1.5rem;
  gap: 1.5rem;
}

.order-info-left {
  display: flex;
  gap: 1.5rem;
  flex: 1;
}

.info-box {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  padding: 1rem 1.25rem;
  background: #f9fafb;
  border-radius: 8px;
  min-width: 280px;
}

.info-label {
  font-size: 0.875rem;
  color: #6b7280;
  font-weight: 500;
}

.info-value {
  font-size: 0.95rem;
  font-weight: 600;
  color: #1f2937;
}

.total-amount {
  font-size: 1.25rem;
  color: #e74c3c;
  font-weight: 700;
}

.payment-method {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: #1f2937;
  font-weight: 600;
}

.payment-method i {
  font-size: 1.1rem;
  color: #6b7280;
}

.order-actions {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
  justify-content: center;
}

.btn-view-detail,
.btn-cancel-order {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  padding: 0.6rem 1.5rem;
  border: none;
  border-radius: 8px;
  font-weight: 600;
  font-size: 0.9rem;
  cursor: pointer;
  transition: all 0.3s ease;
  white-space: nowrap;
  min-width: 220px;
}

.btn-view-detail {
  background: white;
  color: #3b82f6;
  border: 2px solid #e5e7eb;
}

.btn-view-detail:hover {
  background: #f0f7ff;
  border-color: #3b82f6;
  transform: translateY(-1px);
  box-shadow: 0 2px 8px rgba(59, 130, 246, 0.15);
}

.btn-cancel-order {
  background: white;
  color: #e74c3c;
  border: 2px solid #e5e7eb;
}

.btn-cancel-order:hover {
  background: #fef2f2;
  color: #e74c3c;
  border-color: #e74c3c;
  transform: translateY(-1px);
  box-shadow: 0 2px 8px rgba(231, 76, 60, 0.15);
}

.btn-view-detail i,
.btn-cancel-order i {
  font-size: 1rem;
}

@media (max-width: 992px) {
  .order-body {
    flex-direction: column;
    gap: 1.25rem;
  }

  .order-info-left {
    flex-direction: column;
    gap: 1rem;
  }

  .info-box {
    min-width: auto;
  }

  .order-actions {
    width: 100%;
  }

  .btn-view-detail,
  .btn-cancel-order {
    width: 100%;
    min-width: auto;
  }
}

@media (max-width: 768px) {
  .order-header {
    flex-direction: column;
    align-items: flex-start;
    gap: 0.75rem;
    padding: 1rem 1.25rem;
  }

  .order-meta {
    width: 100%;
    justify-content: space-between;
  }

  .order-body {
    padding: 1.25rem;
  }

  .order-info-left {
    gap: 0.75rem;
  }

  .info-box {
    padding: 0.875rem 1rem;
  }
}

@media (max-width: 576px) {
  .order-header {
    padding: 0.875rem 1rem;
  }

  .order-code {
    font-size: 0.9rem;
  }

  .order-date {
    font-size: 0.85rem;
  }

  .status-badge {
    font-size: 0.75rem;
    padding: 0.3rem 0.7rem;
  }

  .order-body {
    padding: 1rem;
  }

  .info-box {
    padding: 0.75rem 0.875rem;
  }

  .info-label {
    font-size: 0.8rem;
  }

  .info-value {
    font-size: 0.875rem;
  }

  .total-amount {
    font-size: 1.15rem;
  }

  .btn-view-detail,
  .btn-cancel-order {
    padding: 0.65rem 1.25rem;
    font-size: 0.85rem;
  }
}
</style>