<template>
  <div class="cart-page">
    <div class="container">
      <nav aria-label="breadcrumb-cart">
        <ol class="breadcrumb">
          <li class="breadcrumb-item">
            <router-link to="/">Trang chủ</router-link>
          </li>
          <li class="breadcrumb-item">
            <router-link to="/gio-hang">Giỏ hàng</router-link>
          </li>
        </ol>
      </nav>
      <div v-if="itemCount > 0">
        <div class="cart-table-wrapper">
          <div class="table-header-actions">
            <h2 class="cart-title">GIỎ HÀNG CỦA BẠN</h2>
            <button @click="clearAll" class="btn-clear-cart">
              <i class="bi bi-trash"></i> Xóa tất cả
            </button>
          </div>
          <div class="table-responsive">
            <table class="cart-table">
              <thead>
                <tr>
                  <th class="table-head-text product-name-head">Sản phẩm</th>
                  <th class="table-head-text">Kích thước</th>
                  <th class="table-head-text">Màu sắc</th>
                  <th class="table-head-text">Đơn giá</th>
                  <th class="table-head-text">Số lượng</th>
                  <th class="table-head-text">Thành tiền</th>
                  <th class="table-head-text">Xóa</th>
                </tr>
              </thead>
              <tbody>
                <CartItem
                  v-for="(item, key) in items"
                  :key="key"
                  :item="item"
                  :item-key="key"
                />
              </tbody>
            </table>
          </div>
          <div class="cart-footer-actions">
            <router-link to="/san-pham" class="btn-continue-shopping">
                <i class="bi bi-arrow-left"></i> Tiếp tục mua sắm
            </router-link>
            <div class="cart-total-section">
              <div class="total-info">
                <span class="total-label">Tổng cộng:</span>
                <span class="total-amount">{{ formatPrice(totalAmount) }}₫</span>
              </div>
              <router-link to="/thanh-toan" class="btn-checkout">
                <i class="bi bi-credit-card"></i> TIẾN HÀNH THANH TOÁN
              </router-link>
            </div>
          </div>
        </div>
      </div>
      <div v-else class="cart-empty">
        <div class="empty-cart-content">
          <div class="empty-icon">
            <i class="bi bi-cart-x"></i>
          </div>
          <h4 class="empty-title">Giỏ hàng của bạn đang trống</h4>
          <p class="empty-text">Hãy thêm sản phẩm vào giỏ hàng để tiếp tục mua sắm!</p>
          <router-link to="/san-pham" class="btn-shop-now">
            <i class="bi bi-bag-check"></i> Mua sắm ngay
          </router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'
import Swal from 'sweetalert2'
import { useCartStore } from '@/stores/cart'
import CartItem from '@/components/cart/CartItem.vue'

const cartStore = useCartStore()
const items = computed(() => cartStore.items)
const itemCount = computed(() => cartStore.itemCount)

const totalAmount = computed(() => {
  return Object.values(items.value).reduce((sum, item) => {
    return sum + (item.price * item.quantity)
  }, 0)
})

const formatPrice = (price) => new Intl.NumberFormat('vi-VN').format(price)

const clearAll = async () => {
  const result = await Swal.fire({
    icon: 'warning',
    title: 'Xóa toàn bộ giỏ hàng?',
    html: `
      <p>Bạn có chắc muốn <strong>xóa tất cả sản phẩm</strong> trong giỏ hàng?</p>
      <p style="color:#e74c3c; font-weight:600;">Hành động này không thể hoàn tác.</p>
    `,
    showCancelButton: true,
    confirmButtonText: 'Xóa tất cả',
    cancelButtonText: 'Hủy',
    confirmButtonColor: '#dc3545'
  })
  if (result.isConfirmed) {
    cartStore.clearCart()
    await Swal.fire({
      icon: 'success',
      title: 'Đã xóa giỏ hàng',
      text: 'Tất cả sản phẩm đã được xóa khỏi giỏ hàng.'
    })
  }
}
</script>

<style scoped>
.cart-page {
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

.cart-table-wrapper {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0,0,0,0.08);
  overflow: hidden;
}

.table-header-actions {
  display: flex;
  align-items: center;
  padding: 1.75rem;
  border-bottom: 2px solid #f0f0f0;
  position: relative;
  justify-content: center; 
}

.cart-title {
  font-weight: 700;
  font-size: 1.4rem;
  color: #34a853;
  margin: 0;
  letter-spacing: 0.5px;
}

.btn-clear-cart {
  position: absolute;
  right: 1.5rem;
  padding: 0.5rem 1rem;
  border-radius: 6px;
  font-weight: 600;
  font-size: 0.875rem;
  transition: all 0.3s ease;
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  border: none;
  cursor: pointer;
  background: white;
  color: #dc3545;
  border: 1.5px solid #dc3545;
}

.btn-clear-cart:hover {
  background: #dc3545;
  color: white;
  transform: translateY(-1px);
  box-shadow: 0 3px 10px rgba(220, 53, 69, 0.25);
}

.btn-clear-cart i {
  font-size: 0.95rem;
}

.table-responsive {
  overflow-x: auto;
}

.cart-table {
  width: 100%;
  margin-bottom: 0;
}

.cart-table thead {
  background: #f8f9fa;
  border-bottom: 2px solid #e9ecef;
}

.cart-table thead tr th {
  color: #495057;
  font-weight: 600;
  font-size: 0.875rem;
  padding: 0.875rem 0.75rem;
  text-align: center;
  border: none;
  white-space: nowrap;
  text-transform: uppercase;
  letter-spacing: 0.3px;
}

.table-head-text:first-child {
  padding-left: 1.5rem;
}

.product-name-head {
  min-width: 200px;
}

.cart-footer-actions {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.25rem 1.5rem;
  background: #fafafa;
  border-top: 1px solid #f0f0f0;
  gap: 1.5rem;
}

.btn-continue-shopping {
  padding: 0.65rem 1.5rem;
  border-radius: 6px;
  font-weight: 600;
  font-size: 0.875rem;
  transition: all 0.3s ease;
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  text-decoration: none;
  background: white;
  color: #495057;
  border: 1.5px solid #dee2e6;
}

.btn-continue-shopping:hover {
  background: #f8f9fa;
  color: #3b82f6;
  border-color: #3b82f6;
  transform: translateY(-1px);
}

.btn-continue-shopping i {
  font-size: 0.95rem;
}

.cart-total-section {
  display: flex;
  align-items: center;
  gap: 2rem;
}

.total-info {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.total-label {
  font-size: 0.95rem;
  color: #666;
  font-weight: 500;
}

.total-amount {
  font-size: 1.25rem;
  font-weight: 700;
  color: #dc3545;
}

.btn-checkout {
  padding: 0.75rem 1.75rem;
  background: var(--gradient);
  color: white;
  border: none;
  border-radius: 6px;
  font-weight: 700;
  font-size: 0.8rem;
  transition: all 0.3s ease;
  display: inline-flex;
  align-items: center;
  gap: 0.6rem;
  text-decoration: none;
  cursor: pointer;
  box-shadow: 0 3px 12px rgba(102, 126, 234, 0.3);
  white-space: nowrap;
  letter-spacing: 0.3px;
  text-transform: uppercase;
}

.btn-checkout:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(102, 126, 234, 0.4);
}

.btn-checkout i {
  font-size: 1rem;
}

.cart-empty {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 450px;
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0,0,0,0.08);
}

.empty-cart-content {
  text-align: center;
  max-width: 400px;
  padding: 2.5rem 2rem;
}

.empty-icon {
  font-size: 5rem;
  color: #cbd5e1;
  margin-bottom: 1.5rem;
  animation: emptyCart 2s ease-in-out infinite;
}

@keyframes emptyCart {
  0%, 100% { transform: translateY(0); }
  50% { transform: translateY(-10px); }
}

.empty-title {
  font-size: 1.35rem;
  font-weight: 700;
  color: #2c3e50;
  margin-bottom: 0.75rem;
}

.empty-text {
  color: #666;
  font-size: 0.95rem;
  margin-bottom: 1.75rem;
  line-height: 1.6;
}

.btn-shop-now {
  display: inline-flex;
  align-items: center;
  gap: 0.65rem;
  padding: 0.8rem 1.75rem;
  background: var(--gradient);
  color: white;
  border: none;
  border-radius: 50px;
  font-weight: 600;
  font-size: 1rem;
  text-decoration: none;
  transition: all 0.3s ease;
  box-shadow: 0 3px 12px rgba(102, 126, 234, 0.3);
}

.btn-shop-now:hover {
  transform: translateY(-3px);
  box-shadow: 0 6px 20px rgba(102, 126, 234, 0.4);
}

@media (max-width: 1200px) {
  .cart-page {
    padding: 5px;
  }

  .cart-title {
    font-size: 1.15rem;
  }
  
  .cart-table thead tr th {
    font-size: 0.8rem;
    padding: 0.8rem 0.6rem;
  }
  
  .product-name-head {
    min-width: 180px;
  }
  
  .total-amount {
    font-size: 1.35rem;
  }
}

@media (max-width: 992px) {
  .table-header-actions {
    padding: 1rem 1.25rem;
  }

  .cart-page {
    padding: 5px;
  }
  
  .cart-title {
    font-size: 1.1rem;
  }
  
  .btn-clear-cart {
    padding: 0.4rem 0.9rem;
    font-size: 0.825rem;
  }
  
  .cart-footer-actions {
    flex-direction: column;
    gap: 1.25rem;
    padding: 1.25rem;
  }
  
  .btn-continue-shopping {
    width: 100%;
    justify-content: center;
    order: 2;
  }
  
  .cart-total-section {
    width: 100%;
    flex-direction: column;
    gap: 0.875rem;
    order: 1;
  }
  
  .total-info {
    width: 100%;
    justify-content: space-between;
  }
  
  .btn-checkout {
    width: 100%;
    justify-content: center;
    padding: 0.875rem 1.5rem;
  }
}

@media (max-width: 768px) {
  .breadcrumb-cart {
    font-size: 0.825rem;
  }
  
  .table-header-actions {
    padding: 0.875rem 1rem;
  }

  .cart-page {
    padding: 5px;
  }
  
  .cart-title {
    font-size: 1rem;
  }
  
  .btn-clear-cart {
    padding: 0.35rem 0.95rem;
    font-size: 0.8rem;
  }
  
  .cart-table thead tr th {
    font-size: 0.75rem;
    padding: 0.7rem 0.4rem;
  }
  
  .table-head-text:first-child {
    padding-left: 1rem;
  }
  
  .product-name-head {
    min-width: 160px;
  }
  
  .cart-footer-actions {
    padding: 1rem;
  }
  
  .total-label {
    font-size: 0.875rem;
  }
  
  .total-amount {
    font-size: 1.25rem;
  }
  
  .btn-continue-shopping,
  .btn-checkout {
    padding: 0.75rem 1.25rem;
    font-size: 0.85rem;
  }
  
  .empty-icon {
    font-size: 4rem;
  }
  
  .empty-title {
    font-size: 1.15rem;
  }
  
  .empty-text {
    font-size: 0.875rem;
  }
  
  .btn-shop-now {
    padding: 0.75rem 1.75rem;
    font-size: 0.95rem;
  }
}

@media (max-width: 576px) {
  .container {
    padding: 0.75rem;
  }

  .cart-page {
    padding: 5px;
  }
  
  .breadcrumb-cart {
    font-size: 0.8rem;
    margin-bottom: 0.75rem;
  }
  
  .cart-table-wrapper {
    border-radius: 6px;
  }
  
  .table-header-actions {
    padding: 0.75rem 0.875rem;
    flex-wrap: wrap;
    gap: 0.75rem;
  }
  
  .cart-title {
    font-size: 0.95rem;
    width: 100%;
    margin-bottom: 0.5rem;
  }
  
  .btn-clear-cart {
    padding: 0.35rem 0.8rem;
    font-size: 0.75rem;
    margin-left: auto;
  }
  
  .cart-table thead tr th {
    font-size: 0.7rem;
    padding: 0.6rem 0.3rem;
  }
  
  .table-head-text:first-child {
    padding-left: 0.75rem;
  }
  
  .product-name-head {
    min-width: 140px;
  }
  
  .cart-footer-actions {
    padding: 0.875rem;
    gap: 1rem;
  }
  
  .cart-total-section {
    gap: 0.75rem;
  }
  
  .total-label {
    font-size: 0.825rem;
  }
  
  .total-amount {
    font-size: 1.15rem;
  }
  
  .btn-continue-shopping,
  .btn-checkout {
    padding: 0.65rem 1.1rem;
    font-size: 0.8rem;
  }
  
  .btn-checkout i,
  .btn-continue-shopping i {
    font-size: 0.9rem;
  }
  
  .empty-cart-content {
    padding: 1.75rem 1rem;
  }
  
  .empty-icon {
    font-size: 3.25rem;
  }
  
  .empty-title {
    font-size: 1rem;
  }
  
  .empty-text {
    font-size: 0.825rem;
    margin-bottom: 1.5rem;
  }
  
  .btn-shop-now {
    padding: 0.65rem 1.5rem;
    font-size: 0.875rem;
  }
}

@media (max-width: 400px) {
  .cart-title {
    font-size: 0.9rem;
  }
  
  .btn-clear-cart {
    padding: 0.35rem 0.8rem;
    font-size: 0.7rem;
  }
  
  .total-amount {
    font-size: 1.05rem;
  }
  
  .btn-continue-shopping,
  .btn-checkout {
    padding: 0.6rem 1rem;
    font-size: 0.75rem;
    gap: 0.4rem;
  }
}
</style>