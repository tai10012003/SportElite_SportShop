<template>
  <Teleport to="body">
    <Transition name="modal">
      <div v-if="isOpen" class="modal-overlay" @click="closeModal">
        <div class="modal-container" @click.stop>
          <button class="modal-close" @click="closeModal">
            <i class="bi bi-x-lg"></i>
          </button>

          <div v-if="isLoading" class="modal-loading">
            <div class="spinner-border text-primary" role="status"></div>
            <p>Đang tải thông tin đơn hàng...</p>
          </div>

          <div v-else-if="order" class="modal-content">
            <div class="modal-header">
              <h2 class="modal-title">Chi tiết đơn hàng</h2>
              <div class="order-code-badge">{{ order.maDonHang }}</div>
            </div>

            <div class="modal-body">
              <div class="section">
                <h3 class="section-title">
                  <i class="bi bi-info-circle"></i>
                  Thông tin đơn hàng
                </h3>
                <div class="info-grid">
                  <div class="info-row">
                    <span class="info-label">Trạng thái:</span>
                    <span :class="['status-badge', getStatusClass(order.trangThai)]">
                      {{ getStatusText(order.trangThai) }}
                    </span>
                  </div>
                  <div class="info-row">
                    <span class="info-label">Ngày đặt:</span>
                    <span class="info-value">{{ formatDate(order.ngayTao) }}</span>
                  </div>
                  <div class="info-row">
                    <span class="info-label">Thanh toán:</span>
                    <span class="info-value">{{ getPaymentMethodText(order.phuongThucThanhToan) }}</span>
                  </div>
                  <div class="info-row">
                    <span class="info-label">Đã thanh toán:</span>
                    <span :class="['payment-status', order.daThanhToan ? 'paid' : 'unpaid']">
                      {{ order.daThanhToan ? 'Rồi' : 'Chưa' }}
                    </span>
                  </div>
                </div>
              </div>

              <div class="section">
                <h3 class="section-title">
                  <i class="bi bi-person"></i>
                  Thông tin nhận hàng
                </h3>
                <div class="info-grid">
                  <div class="info-row">
                    <span class="info-label">Người nhận:</span>
                    <span class="info-value">{{ order.tenNguoiNhan }}</span>
                  </div>
                  <div class="info-row">
                    <span class="info-label">Số điện thoại:</span>
                    <span class="info-value">{{ order.soDienThoai }}</span>
                  </div>
                  <div class="info-row full-width">
                    <span class="info-label">Địa chỉ:</span>
                    <span class="info-value">{{ order.diaChi }}</span>
                  </div>
                  <div v-if="order.ghiChu" class="info-row full-width">
                    <span class="info-label">Ghi chú:</span>
                    <span class="info-value">{{ order.ghiChu }}</span>
                  </div>
                </div>
              </div>

              <div class="section">
                <h3 class="section-title">
                  <i class="bi bi-cart"></i>
                  Sản phẩm
                </h3>
                <div class="order-items">
                  <div v-for="item in order.items" :key="item.id" class="order-item">
                    <img 
                      :src="item.hinhAnh || '/images/no-image.jpg'" 
                      :alt="item.tenSanPham" 
                      class="order-item-image"
                    >
                    <div class="order-item-info">
                      <h6 class="order-item-name">{{ item.tenSanPham }}</h6>
                      <div class="order-item-details">
                        <div class="order-item-meta">
                          <span v-if="item.mauSac" class="meta-badge">{{ item.mauSac }}</span>
                          <span v-if="item.kichThuoc" class="meta-badge">{{ item.kichThuoc }}</span>
                        </div>
                        <div class="order-item-bottom">
                          <span class="item-quantity">Số lượng: {{ item.soLuong }}</span>
                          <span class="item-price">{{ formatPrice(item.thanhTien) }}₫</span>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>

              <div class="section">
                <h3 class="section-title">
                  <i class="bi bi-receipt"></i>
                  Tổng thanh toán
                </h3>
                <div class="payment-summary">
                  <div class="summary-row">
                    <span>Tạm tính:</span>
                    <span>{{ formatPrice(order.tamTinh) }}₫</span>
                  </div>
                  <div class="summary-row">
                    <span>Phí vận chuyển:</span>
                    <span :class="{ 'text-success': order.phiVanChuyen === 0 }">
                      {{ order.phiVanChuyen === 0 ? 'Miễn phí' : formatPrice(order.phiVanChuyen) + '₫' }}
                    </span>
                  </div>
                  <div v-if="order.soTienGiam > 0" class="summary-row discount">
                    <span>Giảm giá:</span>
                    <span>-{{ formatPrice(order.soTienGiam) }}₫</span>
                  </div>
                  <div class="summary-row total">
                    <span>Tổng cộng:</span>
                    <span class="total-amount">{{ formatPrice(order.tongThanhToan) }}₫</span>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </Transition>
  </Teleport>
</template>

<script setup>
import { ref, watch } from 'vue'
import OrderService from '@/services/OrderService'

const props = defineProps({
  isOpen: Boolean,
  orderId: Number
})

const emit = defineEmits(['close', 'order-updated'])

const order = ref(null)
const isLoading = ref(false)

watch(() => props.isOpen, async (newVal) => {
  if (newVal && props.orderId) {
    await loadOrderDetail()
    document.body.style.overflow = 'hidden'
  } else {
    document.body.style.overflow = ''
  }
})

const loadOrderDetail = async () => {
  isLoading.value = true
  try {
    const data = await OrderService.getOrderById(props.orderId)
    order.value = data
  } catch (error) {
    console.error('Error loading order detail:', error)
  } finally {
    isLoading.value = false
  }
}

const closeModal = () => {
  emit('close')
  order.value = null
}

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
    1: 'Đã xác nhận',
    2: 'Đang giao hàng',
    3: 'Đã hoàn thành',
    4: 'Đã hủy'
  }
  return statusMap[status] || 'Không xác định'
}

const getPaymentMethodText = (method) => {
  const methodMap = {
    'cod': 'COD (Thanh toán khi nhận hàng)',
    'vnpay': 'VNPay',
    'momo': 'MoMo'
  }
  return methodMap[method?.toLowerCase()] || method
}
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.6);
  backdrop-filter: blur(4px);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 9999;
  padding: 1rem;
}

.modal-container {
  background: white;
  border-radius: 16px;
  max-width: 900px;
  width: 100%;
  max-height: 90vh;
  overflow-y: auto;
  position: relative;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
  animation: slideUp 0.3s ease-out;
}

@keyframes slideUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.modal-close {
  position: absolute;
  top: 1rem;
  right: 1rem;
  width: 36px;
  height: 36px;
  border-radius: 50%;
  background: white;
  border: none;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  z-index: 10;
  transition: all 0.3s ease;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.modal-close:hover {
  background: #f8f9fa;
  transform: rotate(90deg);
}

.modal-close i {
  font-size: 1.2rem;
  color: #333;
}

.modal-loading {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 4rem 2rem;
}

.modal-loading p {
  margin-top: 1rem;
  color: #6b7280;
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem 1.5rem;
  border-bottom: 2px solid #e9ecef;
  background: #f8f9fa;
}

.modal-title {
  font-size: 1.4rem;
  font-weight: 700;
  color: #1f2937;
  margin: 0;
}

.order-code-badge {
  padding: 0.5rem 1rem;
  background: #3b82f6;
  color: white;
  border-radius: 8px;
  font-weight: 700;
  font-size: 0.95rem;
  margin-right: 40px;
}

.modal-body {
  padding: 1.5rem 2rem 2rem;
}

.section {
  margin-bottom: 2rem;
}

.section:last-child {
  margin-bottom: 0;
}

.section-title {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  font-size: 1.1rem;
  font-weight: 700;
  color: #1f2937;
  margin-bottom: 1rem;
  padding-bottom: 0.75rem;
  border-bottom: 2px solid #e9ecef;
}

.section-title i {
  color: #3b82f6;
  font-size: 1.25rem;
}

.info-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 1rem;
}

.info-row {
  display: flex;
  gap: 8px;
  align-items: center;
  padding: 0.75rem 1rem;
  background: #f9fafb;
  border-radius: 8px;
}

.info-row.full-width {
  grid-column: 1 / -1;
  flex-direction: column;
  align-items: flex-start;
  gap: 0.5rem;
}

.info-label {
  font-weight: 600;
  color: #6b7280;
  font-size: 0.9rem;
}

.info-value {
  color: #1f2937;
  font-weight: 500;
  text-align: right;
}

.info-row.full-width .info-value {
  text-align: left;
}

.status-badge {
  padding: 0.35rem 0.85rem;
  border-radius: 20px;
  font-size: 0.8rem;
  font-weight: 600;
  text-transform: uppercase;
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

.payment-status {
  padding: 0.25rem 0.75rem;
  border-radius: 12px;
  font-size: 0.85rem;
  font-weight: 600;
}

.payment-status.paid {
  background: #d1fae5;
  color: #059669;
}

.payment-status.unpaid {
  background: #fee2e2;
  color: #dc2626;
}

.order-items {
  max-height: 420px;
  overflow-y: auto;
  padding-right: 0.25rem;
}

.order-items::-webkit-scrollbar {
  width: 6px;
}

.order-items::-webkit-scrollbar-track {
  background: #f5f5f5;
  border-radius: 3px;
}

.order-items::-webkit-scrollbar-thumb {
  background: #ddd;
  border-radius: 3px;
}

.order-items::-webkit-scrollbar-thumb:hover {
  background: #ccc;
}

.order-item {
  display: flex;
  gap: 0.875rem;
  padding: 0.875rem;
  border: 1px solid #e8e8e8;
  border-radius: 8px;
  margin-bottom: 0.875rem;
  transition: all 0.3s ease;
  background: white;
}

.order-item:hover {
  border-color: #d0d0d0;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
}

.order-item:last-child {
  margin-bottom: 0;
}

.order-item-image {
  width: 100px;
  height: 100px;
  object-fit: cover;
  border-radius: 6px;
  border: 1px solid #e8e8e8;
  flex-shrink: 0;
}

.order-item-info {
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.order-item-name {
  font-size: 1rem;
  font-weight: 600;
  color: #333;
  margin: 0;
  line-height: 1.3;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.order-item-details {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.order-item-meta {
  display: flex;
  gap: 0.5rem;
  flex-wrap: wrap;
}

.meta-badge {
  display: inline-block;
  padding: 0.2rem 0.5rem;
  background: #f0f7ff;
  color: #3b82f6;
  border-radius: 4px;
  font-size: 0.83rem;
  font-weight: 500;
}

.order-item-bottom {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.item-quantity {
  font-size: 0.87rem;
  color: #666;
}

.item-price {
  font-size: 1rem;
  font-weight: 700;
  color: #dc3545;
}

.payment-summary {
  background: #f9fafb;
  border-radius: 8px;
  padding: 1rem;
}

.summary-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.75rem 0;
  font-size: 0.95rem;
}

.summary-row:not(:last-child) {
  border-bottom: 1px solid #e5e7eb;
}

.summary-row.discount {
  color: #059669;
  font-weight: 600;
}

.summary-row.total {
  padding-top: 1rem;
  font-size: 1.1rem;
  font-weight: 700;
  color: #1f2937;
}

.total-amount {
  font-size: 1.3rem;
  color: #e74c3c;
}

.text-success {
  color: #059669;
  font-weight: 600;
}

.modal-enter-active,
.modal-leave-active {
  transition: opacity 0.3s ease;
}

.modal-enter-from,
.modal-leave-to {
  opacity: 0;
}

@media (max-width: 768px) {
  .modal-header {
    flex-direction: column;
    align-items: flex-start;
    gap: 1rem;
    padding: 1.5rem 1.25rem;
  }

  .modal-title {
    font-size: 1.25rem;
  }

  .modal-body {
    padding: 1.25rem 1rem;
  }

  .info-grid {
    grid-template-columns: 1fr;
  }

  .order-item {
    padding: 0.75rem;
    gap: 0.75rem;
  }

  .order-item-image {
    width: 60px;
    height: 60px;
  }

  .order-item-name {
    font-size: 0.85rem;
  }
}

@media (max-width: 576px) {
  .modal-container {
    border-radius: 12px;
  }

  .modal-header {
    padding: 1.25rem 1rem;
  }

  .modal-title {
    font-size: 1.15rem;
  }

  .order-code-badge {
    font-size: 0.85rem;
    padding: 0.4rem 0.75rem;
  }

  .modal-body {
    padding: 1rem 0.75rem;
  }

  .section-title {
    font-size: 1rem;
  }

  .order-item {
    padding: 0.65rem;
    gap: 0.65rem;
  }

  .order-item-image {
    width: 55px;
    height: 55px;
  }

  .order-item-name {
    font-size: 0.8rem;
  }

  .meta-badge {
    font-size: 0.7rem;
    padding: 0.15rem 0.4rem;
  }

  .item-quantity {
    font-size: 0.75rem;
  }

  .item-price {
    font-size: 0.8rem;
  }

  .summary-row.total {
    font-size: 1rem;
  }

  .total-amount {
    font-size: 1.15rem;
  }
}
</style>