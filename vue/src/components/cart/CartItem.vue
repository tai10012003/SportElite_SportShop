<template>
  <tr class="cart-item">
    <td class="product-col">
      <div class="product-info">
          <router-link :to="`/san-pham/${item.slug}`">
            <img :src="item.image" :alt="item.name" class="cart-item-image">
          </router-link>
          <div class="product-details">
            <router-link :to="`/san-pham/${item.slug}`" class="cart-item-name">
              {{ item.name }}
            </router-link>
            <div class="product-sku">
              Mã SP: {{ item.id }}
            </div>
          </div>
      </div>
    </td>
    <td class="size-col">
      <span v-if="item.size" class="size-badge">{{ item.size }}</span>
      <span v-else class="text-muted">-</span>
    </td>
    <td class="color-col">
      <div v-if="item.color" class="color-display">
        <span class="color-circle" :style="getColorStyle(item.color)"></span>
        <span class="color-text">{{ item.color }}</span>
      </div>
      <span v-else class="text-muted">-</span>
    </td>
    <td class="price-col">
      <span class="cart-item-price">{{ formatPrice(item.price) }}₫</span>
    </td>
    <td class="quantity-col">
      <div class="cart-quantity-control">
        <button @click="updateQuantity(-1)" class="btn-quantity">-</button>
        <input type="number" :value="quantity" @input="handleInput" min="1" class="quantity-input">
        <button @click="updateQuantity(1)" class="btn-quantity">+</button>
      </div>
    </td>
    <td class="total-col">
      <span class="cart-item-total">{{ formatPrice(item.price * quantity) }}₫</span>
    </td>
    <td class="action-col">
      <button @click="remove" class="btn-remove" title="Xóa sản phẩm">
        <i class="bi bi-trash"></i>
      </button>
    </td>
  </tr>
</template>

<script setup>
import { ref, watch, onMounted } from 'vue'
import { useCartStore } from '@/stores/cart'
import { getColorStyle } from '@/assets/js/colorMap'

const props = defineProps({
  item: Object,
  itemKey: String
})

const cartStore = useCartStore()
const quantity = ref(0)

onMounted(() => {
  quantity.value = props.item.quantity
})

watch(() => props.item.quantity, (newVal) => {
  quantity.value = newVal
})

const handleInput = (e) => {
  const newQty = parseInt(e.target.value)
  if (!isNaN(newQty) && newQty >= 1) {
    updateQuantityDirect(newQty)
  }
}

const updateQuantity = (change) => {
  let newQty = quantity.value + change
  if (newQty < 1) {
    if (confirm('Bạn có chắc muốn xóa sản phẩm này khỏi giỏ hàng?')) {
      cartStore.removeItem(props.itemKey)
    }
    return
  }
  cartStore.updateQuantity(props.itemKey, newQty)
  quantity.value = newQty
}

const updateQuantityDirect = (newQty) => {
  cartStore.updateQuantity(props.itemKey, newQty)
  quantity.value = newQty
}

const remove = () => {
  if (confirm('Bạn có chắc muốn xóa sản phẩm này khỏi giỏ hàng?')) {
    cartStore.removeItem(props.itemKey)
  }
}

const formatPrice = (price) => new Intl.NumberFormat('vi-VN').format(price)
</script>

<style scoped>
.cart-item {
  border-bottom: 1px solid #f0f0f0;
  transition: background-color 0.3s ease;
}

.cart-item:hover {
  background-color: #fafafa;
}

.cart-item td {
  padding: 1.25rem 0.75rem;
  vertical-align: middle;
}

.product-col {
  min-width: 300px;
}

.product-info {
  display: flex;
  gap: 1rem;
  align-items: center;
}

.cart-item-image {
  width: 80px;
  height: 80px;
  object-fit: cover;
  border-radius: 12px;
  border: 1px solid #e8e8e8;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.cart-item-image:hover {
  transform: scale(1.05);
  box-shadow: 0 4px 12px rgba(0,0,0,0.1);
}

.product-details {
  flex: 1;
}

.cart-item-name {
  font-weight: 600;
  color: #333;
  text-decoration: none;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
  line-height: 1.4;
  margin-bottom: 0.5rem;
  transition: color 0.3s ease;
}

.cart-item-name:hover {
  color: #3b82f6;
}

.product-sku {
  font-size: 0.85rem;
  color: #999;
  margin-top: 0.25rem;
}

.size-col {
  min-width: 100px;
  text-align: center;
}

.size-badge {
  display: inline-block;
  padding: 0.4rem 1rem;
  background: #f0f7ff;
  color: #3b82f6;
  border-radius: 20px;
  font-weight: 600;
  font-size: 0.9rem;
  border: 1px solid #d0e7ff;
}

.color-col {
  min-width: 120px;
}

.color-display {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  justify-content: center;
}

.color-circle {
  width: 28px;
  height: 28px;
  border-radius: 50%;
  border: 2px solid #ddd;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}

.color-text {
  font-size: 0.9rem;
  color: #666;
  font-weight: 500;
}

.price-col {
  min-width: 120px;
  text-align: center;
}

.cart-item-price {
  color: #333;
  font-size: 1rem;
  font-weight: 600;
}

.quantity-col {
  min-width: 140px;
}

.cart-quantity-control {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
}

.btn-quantity {
  width: 32px;
  height: 32px;
  border: 1px solid #ddd;
  background: white;
  border-radius: 8px;
  font-weight: bold;
  color: #333;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  justify-content: center;
}

.btn-quantity:hover {
  background: #3b82f6;
  color: white;
  border-color: #3b82f6;
  transform: scale(1.1);
}

.btn-quantity:active {
  transform: scale(0.95);
}

.quantity-input {
  width: 60px;
  text-align: center;
  border: 1px solid #ddd;
  border-radius: 8px;
  padding: 0.25rem;
  font-weight: 600;
  font-size: 0.95rem;
}

.quantity-input:focus {
  outline: none;
  border-color: #3b82f6;
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
}

.total-col {
  min-width: 130px;
  text-align: center;
}

.cart-item-total {
  font-weight: 700;
  color: #e74c3c;
  font-size: 1.15rem;
}

.action-col {
  min-width: 80px;
  text-align: center;
}

.btn-remove {
  background: none;
  border: none;
  color: #999;
  font-size: 1.3rem;
  padding: 0.5rem;
  cursor: pointer;
  transition: all 0.3s ease;
  border-radius: 8px;
}

.btn-remove:hover {
  color: #e74c3c;
  background: #fff5f5;
  transform: scale(1.1);
}

@media (max-width: 1200px) {
  .product-col {
    min-width: 250px;
  }
  
  .cart-item-image {
    width: 70px;
    height: 70px;
  }
  
  .cart-item-name {
    font-size: 0.95rem;
  }
}

@media (max-width: 992px) {
  .cart-item td {
    padding: 1rem 0.5rem;
  }
  
  .product-col {
    min-width: 200px;
  }
  
  .size-badge {
    padding: 0.3rem 0.8rem;
    font-size: 0.85rem;
  }
  
  .color-circle {
    width: 24px;
    height: 24px;
  }
  
  .color-text {
    font-size: 0.85rem;
  }
}
</style>