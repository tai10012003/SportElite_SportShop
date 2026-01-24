<template>
  <div class="order-summary-sticky">
    <div class="card shadow-sm border-0">
      <div class="card-header bg-white">
        <h5 class="card-title mb-0 py-2">
          <i class="bi bi-bag-check me-2"></i>ĐƠN HÀNG CỦA BẠN
        </h5>
      </div>
      <div class="card-body">
        <div class="order-items">
          <div 
            v-for="(item, index) in cartItems" 
            :key="index"
            class="order-item"
          >
            <img 
              :src="getItemImage(item)" 
              :alt="item.name" 
              class="order-item-image"
            >
            <div class="order-item-info">
              <h6 class="order-item-name">{{ item.name }}</h6>
              <div class="order-item-details">
                <div class="order-item-meta">
                  <span v-if="item.color" class="meta-badge">{{ item.color }}</span>
                  <span v-if="item.size" class="meta-badge">{{ item.size }}</span>
                </div>
                <div class="order-item-bottom">
                  <span class="item-quantity">Số lượng: {{ item.quantity }}</span>
                  <span class="item-price">{{ formatPrice(item.price * item.quantity) }}₫</span>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="order-totals">
          <div class="total-row">
            <span>Tạm tính</span>
            <span>{{ formatPrice(subtotal) }}₫</span>
          </div>
          <div class="total-row">
            <span>Phí vận chuyển</span>
            <span :class="{ 'text-success fw-bold': shippingFee === 0 }">
              {{ shippingFee > 0 ? formatPrice(shippingFee) + '₫' : 'Miễn phí' }}
            </span>
          </div>
          <div class="total-row total-final">
            <span>Tổng cộng</span>
            <span class="total-amount">{{ formatPrice(totalAmount) }}₫</span>
          </div>
        </div>
        <button 
          type="button" 
          class="btn-place-order"
          @click="handlePlaceOrder"
          :disabled="isProcessing"
        >
          <span v-if="isProcessing" class="spinner-border spinner-border-sm me-2"></span>
          <span v-else>ĐẶT HÀNG NGAY</span>
          <i v-if="!isProcessing" class="bi bi-arrow-right ms-2"></i>
        </button>
      </div>
    </div>
    <div class="card shadow-sm border-0 mt-3 security-notice">
      <div class="card-body">
        <div class="security-content">
          <i class="bi bi-shield-check"></i>
          <div>
            <h6>Thông tin bảo mật</h6>
            <p>Thông tin thanh toán của bạn được mã hóa và bảo mật an toàn.</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed, ref } from 'vue'
import { useCartStore } from '@/stores/cart'

const cartStore = useCartStore()
const isProcessing = ref(false)

const emit = defineEmits(['place-order'])

const cartItems = computed(() => {
  return Object.values(cartStore.items)
})

const subtotal = computed(() => {
  return cartStore.subtotal
})

const shippingFee = computed(() => {
  return subtotal.value >= 500000 ? 0 : 30000
})

const totalAmount = computed(() => {
  return subtotal.value + shippingFee.value
})

const formatPrice = (price) => {
  return new Intl.NumberFormat('vi-VN').format(price)
}

const getItemImage = (item) => {
  return item.image || 'https://res.cloudinary.com/df1wrn1az/image/upload/v1768964222/no-image_v1ltyr.png'
}

const handlePlaceOrder = () => {
  isProcessing.value = true
  emit('place-order', {
    subtotal: subtotal.value,
    shippingFee: shippingFee.value,
    totalAmount: totalAmount.value
  })
  setTimeout(() => {
    isProcessing.value = false
  }, 2000)
}
</script>

<style scoped>
.order-summary-sticky {
  position: sticky;
  top: 90px;
}

.card {
  border-radius: 8px;
  overflow: hidden;
}

.card-header {
  padding: 1rem 1.25rem;
  border-bottom: 2px solid #f0f0f0;
}

.card-title {
  font-size: 1.2rem;
  font-weight: 700;
  color: #333;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  display: flex;
  align-items: center;
}

.card-title i {
  font-size: 1.2rem;
  color: #666;
}

.card-body {
  padding: 1.25rem;
}

.order-items {
  max-height: 380px;
  overflow-y: auto;
  margin-bottom: 1.25rem;
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
}

.order-item:hover {
  border-color: #d0d0d0;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
}

.order-item:last-child {
  margin-bottom: 0;
}

.order-item-image {
  width: 80px;
  height: 80px;
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

.order-totals {
  border-top: 2px solid #f0f0f0;
  padding-top: 1rem;
  margin-bottom: 1.25rem;
}

.total-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.5rem 0;
  font-size: 1rem;
  color: #666;
}

.total-row span:last-child {
  font-weight: 600;
  color: #333;
}

.total-final {
  border-top: 2px solid #f0f0f0;
  padding-top: 1rem;
  margin-top: 0.75rem;
  font-size: 1rem;
}

.total-final span:first-child {
  font-weight: 700;
  color: #333;
}

.total-amount {
  font-size: 1.25rem !important;
  font-weight: 700 !important;
  color: #dc3545 !important;
}

.btn-place-order {
  width: 100%;
  padding: 0.7rem;
  background: var(--gradient);
  color: white;
  border: none;
  border-radius: 8px;
  font-weight: 700;
  font-size: 1rem;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  box-shadow: 0 4px 15px rgba(102, 126, 234, 0.3);
  cursor: pointer;
}

.btn-place-order:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(102, 126, 234, 0.4);
}

.btn-place-order:disabled {
  opacity: 0.7;
  cursor: not-allowed;
}

.btn-place-order i {
  font-size: 1.1rem;
}

.security-notice {
  background: #f8f9fa;
}

.security-notice .card-body {
  padding: 1rem;
}

.security-content {
  display: flex;
  align-items: flex-start;
  gap: 0.75rem;
}

.security-content i {
  font-size: 1.75rem;
  color: #3b82f6;
  flex-shrink: 0;
}

.security-content h6 {
  font-size: 1rem;
  font-weight: 600;
  color: #333;
  margin: 0 0 0.25rem 0;
}

.security-content p {
  font-size: 0.9rem;
  color: #666;
  margin: 0;
  line-height: 1.4;
}

@media (max-width: 992px) {
  .order-summary-sticky {
    position: relative;
    top: 0;
    margin-top: 2rem;
  }
}

@media (max-width: 768px) {
  .card-header {
    padding: 0.875rem 1rem;
  }
  
  .card-title {
    font-size: 0.95rem;
  }
  
  .card-body {
    padding: 1rem;
  }
  
  .order-items {
    max-height: 320px;
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
  
  .item-quantity {
    font-size: 0.8rem;
  }
  
  .item-price {
    font-size: 0.85rem;
  }
  
  .total-row {
    font-size: 0.85rem;
  }
  
  .total-amount {
    font-size: 1.2rem !important;
  }
  
  .btn-place-order {
    padding: 0.875rem;
    font-size: 0.9rem;
  }
  
  .security-content i {
    font-size: 1.5rem;
  }
  
  .security-content h6 {
    font-size: 0.85rem;
  }
  
  .security-content p {
    font-size: 0.75rem;
  }
}

@media (max-width: 576px) {
  .order-summary-sticky {
    margin-top: 1.5rem;
  }
  
  .card {
    border-radius: 6px;
  }
  
  .card-header {
    padding: 0.75rem 0.875rem;
  }
  
  .card-title {
    font-size: 0.875rem;
  }
  
  .card-title i {
    font-size: 1rem;
  }
  
  .card-body {
    padding: 0.875rem;
  }
  
  .order-items {
    max-height: 280px;
    margin-bottom: 1rem;
  }
  
  .order-item {
    padding: 0.65rem;
    gap: 0.65rem;
    margin-bottom: 0.75rem;
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
  
  .order-totals {
    padding-top: 0.875rem;
    margin-bottom: 1rem;
  }
  
  .total-row {
    font-size: 0.8rem;
    padding: 0.4rem 0;
  }
  
  .total-final {
    font-size: 0.9rem;
    padding-top: 0.875rem;
    margin-top: 0.5rem;
  }
  
  .total-amount {
    font-size: 1.15rem !important;
  }
  
  .btn-place-order {
    padding: 0.8rem;
    font-size: 0.85rem;
  }
  
  .btn-place-order i {
    font-size: 1rem;
  }
  
  .security-notice .card-body {
    padding: 0.875rem;
  }
  
  .security-content {
    gap: 0.65rem;
  }
  
  .security-content i {
    font-size: 1.35rem;
  }
  
  .security-content h6 {
    font-size: 0.8rem;
  }
  
  .security-content p {
    font-size: 0.7rem;
  }
}
</style>