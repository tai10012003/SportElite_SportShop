<template>
  <Teleport to="body">
    <Transition name="modal">
      <div v-if="isOpen" class="modal-overlay" @click="closeModal">
        <div class="modal-container" @click.stop>
          <button class="modal-close" @click="closeModal">
            <i class="bi bi-x-lg"></i>
          </button>
          <div class="modal-content">
            <div class="row">
              <div class="col-md-5">
                <div class="modal-image-wrapper">
                  <img :src="productImage" :alt="product.tenSanPham" class="modal-main-image">
                </div>
              </div>
              <div class="col-md-7">
                <div class="modal-info">
                  <h2 class="modal-title">{{ product.tenSanPham }}</h2>
                  <div class="modal-price">
                    <span class="current-price">{{ formatPrice(product.giaKhuyenMai || product.gia) }}₫</span>
                    <span class="old-price" v-if="product.giaKhuyenMai">{{ formatPrice(product.gia) }}₫</span>
                    <span class="discount-badge" v-if="product.giaKhuyenMai">
                    -{{ Math.round(((product.gia - product.giaKhuyenMai) / product.gia) * 100) }}%
                    </span>
                  </div>
                  <div class="modal-meta">
                    <div class="meta-item">
                      <i class="bi bi-box-seam"></i>
                      <span>Còn {{ product.soLuong }} sản phẩm</span>
                    </div>
                     <div class="meta-item">
                      <span class="rating-wrapper">
                        <div class="star-rating">
                          <i v-for="i in 5" 
                            :key="i" 
                            :class="[
                              'bi', 
                              i <= product.averageRating 
                                ? 'bi-star-fill' 
                                : (i - product.averageRating <= 0.5 
                                  ? 'bi-star-half' 
                                  : 'bi-star'),
                              'star-icon'
                            ]">
                          </i>
                        </div>
                        <span class="rating-text">
                          ({{ product.averageRating.toFixed(1) }}/5 - {{ product.totalReviews }} đánh giá)
                        </span>
                      </span>
                    </div>
                  </div>
                  <div class="modal-quantity">
                    <label class="quantity-label">Số lượng:</label>
                    <div class="quantity-controls">
                    <button @click="decreaseQuantity" class="qty-btn">-</button>
                    <input type="number" v-model="quantity" min="1" :max="product.soLuong" class="qty-input">
                    <button @click="increaseQuantity" class="qty-btn">+</button>
                    </div>
                  </div>
                  <div class="modal-variations" v-if="hasColors">
                    <label class="variation-label">
                        Màu sắc: <span class="text-danger">*</span>
                        <!-- <span v-if="selectedColor" class="selected-text">({{ selectedColor }})</span> -->
                    </label>
                    <div class="color-options">
                      <div v-for="(color, index) in colors" :key="index" class="color-option">
                        <input 
                          type="radio" 
                          :id="'modal-color-' + index" 
                          :value="color"
                          v-model="selectedColor" 
                          class="color-radio"
                        >
                        <label 
                          :for="'modal-color-' + index" 
                          class="color-label"
                          :style="getColorStyle(color)"
                          :title="color"
                        >
                          <span class="color-name">{{ color }}</span>
                        </label>
                      </div>
                    </div>
                  </div>
                  <div class="modal-variations" v-if="hasSizes">
                    <label class="variation-label">
                      Kích thước: <span class="text-danger">*</span>
                      <!-- <span v-if="selectedSize" class="selected-text">({{ selectedSize }})</span> -->
                    </label>
                    <div class="size-options">
                      <div v-for="(size, index) in sizes" :key="index" class="size-option">
                        <input 
                          type="radio" 
                          :id="'modal-size-' + index" 
                          :value="size"
                          v-model="selectedSize" 
                          class="size-radio"
                        >
                        <label :for="'modal-size-' + index" class="size-label">
                          {{ size }}
                        </label>
                      </div>
                    </div>
                  </div>
                  <div class="modal-actions">
                    <button @click="addToCart" class="btn-add-cart">
                      <i class="bi bi-cart-plus"></i> Thêm vào giỏ hàng
                    </button>
                    <router-link :to="`/san-pham/${product.slug}`" class="btn-view-detail">
                      <i class="bi bi-eye"></i> Xem chi tiết
                    </router-link>
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
import { ref, computed, watch } from 'vue'
import Swal from 'sweetalert2'
import { getColorStyle } from '@/assets/js/colorMap'
import { useCartStore } from '@/stores/cart'

const props = defineProps({
  isOpen: Boolean,
  product: {
    type: Object,
    required: true
  }
})

const emit = defineEmits(['close'])
const cartStore = useCartStore()

const quantity = ref(1)
const selectedColor = ref('')
const selectedSize = ref('')

const productImage = computed(() => {
  if (!props.product) return '/images/no-image.jpg'
  if (props.product.mainImage) {
    return props.product.mainImage
  }
  if (props.product.images && props.product.images.length > 0) {
    return props.product.images[0]
  }
  if (props.product.hinhAnh && props.product.hinhAnh.length > 0) {
    const mainImage = props.product.hinhAnh.find(img => img.anhChinh) || props.product.hinhAnh[0]
    return mainImage ? `/uploads/products/${mainImage.duongDan}` : '/images/no-image.jpg'
  }
  return '/images/no-image.jpg'
})

const stockQuantity = computed(() => parseInt(props.product.soLuong) || 0)

const colors = computed(() => {
  return props.product.mauSac?.trim() ? props.product.mauSac.split(',').map(c => c.trim()) : []
})

const sizes = computed(() => {
  return props.product.kichThuoc?.trim() ? props.product.kichThuoc.split(',').map(s => s.trim()) : []
})

const hasColors = computed(() => colors.value.length > 0)
const hasSizes = computed(() => sizes.value.length > 0)

watch(() => props.isOpen, (newVal) => {
  if (newVal) {
    quantity.value = 1
    selectedColor.value = ''
    selectedSize.value = ''
    document.body.style.overflow = 'hidden'
  } else {
    document.body.style.overflow = ''
  }
})

const formatPrice = (price) => new Intl.NumberFormat('vi-VN').format(price)

const closeModal = () => {
  emit('close')
}

const increaseQuantity = () => {
  if (quantity.value < props.product.soLuong) {
    quantity.value++
  }
}

const decreaseQuantity = () => {
  if (quantity.value > 1) {
    quantity.value--
  }
}

const addToCart = async () => {
  if (hasColors.value && !selectedColor.value) {
    await Swal.fire({
      icon: 'warning',
      title: 'Chưa chọn màu sắc',
      text: 'Vui lòng chọn màu sắc trước khi thêm vào giỏ hàng.',
      confirmButtonText: 'Ok'
    })
    return
  }
  if (hasSizes.value && !selectedSize.value) {
    await Swal.fire({
      icon: 'warning',
      title: 'Chưa chọn kích thước',
      text: 'Vui lòng chọn kích thước trước khi thêm vào giỏ hàng.',
      confirmButtonText: 'Ok'
    })
    return
  }
  if (quantity.value > stockQuantity.value) {
    await Swal.fire({
      icon: 'error',
      title: 'Vượt quá tồn kho',
      text: `Số lượng tối đa có thể mua là ${stockQuantity.value} sản phẩm.`,
      confirmButtonText: 'OK'
    })
    return
  }
  cartStore.addItem(
    props.product,
    selectedColor.value,
    selectedSize.value,
    quantity.value
  )
  await Swal.fire({
    icon: 'success',
    title: 'Đã thêm sản phẩm vào giỏ hàng 🎉',
    html: `
      <div style="
        text-align:center;
        line-height:1.7;
        background:#f8f9ff;
        border-radius:12px;
        padding:16px 18px;
        font-size:15px;
      ">
        <div style="margin-bottom:12px;">
          <strong style="font-size:16px; color:#111827;">
            ${props.product.tenSanPham}
          </strong>
        </div>
        <div style="
          display:flex;
          flex-direction:column;
          align-items:center;
          gap:10px;
        ">
          <div style="
            width:220px;
            background:#ffffff;
            padding:8px 12px;
            border-radius:10px;
            border:1px solid #e5e7eb;
            text-align:left;
          ">
            <strong>Số lượng:</strong> ${quantity.value}
          </div>
          ${selectedColor.value ? `
            <div style="
              width:220px;
              background:#ffffff;
              padding:8px 12px;
              border-radius:10px;
              border:1px solid #e5e7eb;
              text-align:left;
            ">
              <strong>Màu sắc:</strong> ${selectedColor.value}
            </div>
          ` : ''}
          ${selectedSize.value ? `
            <div style="
              width:220px;
              background:#ffffff;
              padding:8px 12px;
              border-radius:10px;
              border:1px solid #e5e7eb;
              text-align:left;
            ">
              <strong>Kích thước:</strong> ${selectedSize.value}
            </div>
          ` : ''}
        </div>
      </div>
    `,
    timer: 2800,
    timerProgressBar: true,
    showConfirmButton: false
  })
  closeModal()
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
  border-radius: 20px;
  max-width: 1000px;
  width: 100%;
  max-height: 90vh;
  overflow-y: auto;
  position: relative;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
  animation: modalSlideUp 0.4s cubic-bezier(0.4, 0, 0.2, 1);
}

@keyframes modalSlideUp {
  from {
    opacity: 0;
    transform: translateY(30px) scale(0.95);
  }
  to {
    opacity: 1;
    transform: translateY(0) scale(1);
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

.modal-content {
  padding: 1.5rem;
}

.modal-image-wrapper {
  position: sticky;
  top: 1rem;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
}

.modal-main-image {
  width: 100%;
  object-fit: contain;
}

.modal-info {
  padding-left: 0.5rem;
}

.modal-title {
  font-size: 1.3rem;
  font-weight: 700;
  color: #2c3e50;
  margin-bottom: 1rem;
  line-height: 1.3;
}

.modal-price {
  display: flex;
  align-items: center;
  gap: 1rem;
  margin-bottom: 1rem;
  flex-wrap: wrap;
}

.current-price {
  font-size: 1.3rem;
  font-weight: 600;
  color: #e74c3c;
}

.old-price {
  font-size: 1.1rem;
  color: #999;
  text-decoration: line-through;
}

.discount-badge {
  background: #e74c3c;
  color: white;
  padding: 0.35rem 0.85rem;
  border-radius: 20px;
  font-size: 0.9rem;
  font-weight: 600;
}

.modal-meta {
  display: flex;
  gap: 1.5rem;
  margin-bottom: 1.5rem;
  padding: 1rem;
  background: #f8f9fa;
  border-radius: 12px;
}

.meta-item {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: #666;
  font-size: 0.95rem;
}

.meta-item .bi-box-seam {
  color: #3b82f6;
  font-size: 1.1rem;
}

.modal-quantity {
  display: flex;
  align-items: center;
  gap: 1rem;
  margin-bottom: 1rem;
  padding-bottom: 1rem;
  border-bottom: 1px solid #e8e8e8;
}

.quantity-label {
  font-weight: 600;
  color: #333;
  font-size: 1rem;
}

.quantity-controls {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.qty-btn {
  width: 34px;
  height: 34px;
  border: 1px solid #ddd;
  background: white;
  border-radius: 8px;
  font-weight: 700;
  font-size: 1.1rem;
  color: #333;
  cursor: pointer;
  transition: all 0.3s ease;
}

.qty-btn:hover {
  background: #3b82f6;
  color: white;
  border-color: #3b82f6;
}

.qty-input {
  width: 70px;
  height: 34px;
  text-align: center;
  border: 1px solid #ddd;
  border-radius: 8px;
  font-weight: 600;
  font-size: 1rem;
}

.qty-input:focus {
  outline: none;
  border-color: #3b82f6;
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
}

.modal-variations {
  display: flex;
  gap: 1.5rem;
  margin-bottom: 1rem;
}

.variation-label {
  display: block;
  font-weight: 600;
  color: #333;
  margin: 1rem 0;
  font-size: 1rem;
}

.selected-text {
  color: #3b82f6;
  font-weight: 700;
  margin-left: 0.5rem;
}

.rating-wrapper {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 4px 8px;
  background: #fff8e5;
  border-radius: 20px;
  border: 1px solid #ffe0b2;
}

.star-rating {
  display: flex;
  align-items: center;
  gap: 2px;
}

.star-icon {
  color: #ffa000;
  font-size: 16px;
  transition: all 0.2s ease;
}

.star-icon.bi-star-fill {
  color: #ffa000;
  text-shadow: 0 0 2px rgba(255, 160, 0, 0.3);
}

.star-icon.bi-star-half {
  color: #ffa000;
  position: relative;
  text-shadow: 0 0 2px rgba(255, 160, 0, 0.3);
}

.rating-text {
  font-size: 14px;
  color: #666;
  font-weight: 500;
  white-space: nowrap;
}

.star-rating:hover .star-icon {
  transform: scale(1.1);
}

@keyframes pulse {
  0% { transform: scale(1); }
  50% { transform: scale(1.2); }
  100% { transform: scale(1); }
}

.star-icon.bi-star-fill.text-warning {
  animation: pulse 0.3s ease-in-out;
}

.color-options,
.size-options {
  display: flex;
  flex-wrap: wrap;
  gap: 20px;
  margin-top: 8px;
}

.color-radio {
  display: none;
}

.color-label {
  display: block;
  width: 40px;
  height: 40px;
  border-radius: 50%;
  cursor: pointer;
  position: relative;
  border: 2px solid transparent;
  transition: all 0.3s ease;
}

.color-name {
  position: absolute;
  bottom: -28px;
  left: 50%;
  transform: translateX(-50%);
  font-size: 0.75rem;
  white-space: nowrap;
  color: #666;
  opacity: 0;
  transition: opacity 0.3s ease;
  pointer-events: none;
}

.color-label:hover .color-name {
  opacity: 1;
}

.color-radio:checked + .color-label {
  transform: scale(1.15);
  box-shadow: 0 0 0 3px #fff, 0 0 0 5px #3b82f6;
}

.size-radio {
  display: none;
}


.size-label { 
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 8px;
  padding: 8px 13px;
  border: 2px solid #ddd;
  background: #fff;
  cursor: pointer;
  font-weight: 500;
  color: #333;
  transition: all 0.3s ease;
}

.size-label:hover {
  border-color: #3b82f6;
  color: #3b82f6;
  background: #f0f7ff;
}

.size-radio:checked + .size-label {
  background: #3b82f6;
  border-color: #3b82f6;
  color: white;
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(59, 130, 246, 0.3);
}

@keyframes shake {
  0%, 100% { transform: translateX(0); }
  25% { transform: translateX(-8px); }
  75% { transform: translateX(8px); }
}

.modal-actions {
  display: flex;
  gap: 1rem;
  margin-top: 1.5rem;
}

.btn-add-cart,
.btn-view-detail {
  flex: 1;
  padding: 0.5rem;
  border-radius: 12px;
  font-weight: 700;
  font-size: 1rem;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.65rem;
  transition: all 0.3s ease;
  text-decoration: none;
  border: none;
  cursor: pointer;
}

.btn-add-cart {
  background: var(--gradient);
  color: white;
  box-shadow: 0 4px 15px rgba(102, 126, 234, 0.3);
}

.btn-add-cart:hover {
  transform: translateY(-3px);
}

.btn-view-detail {
  background: white;
  color: #3b82f6;
  border: 2px solid #3b82f6;
}

.btn-view-detail:hover {
  background: #3b82f6;
  color: white;
  transform: translateY(-3px);
  box-shadow: 0 8px 25px rgba(59, 130, 246, 0.3);
}

.btn-add-cart i,
.btn-view-detail i {
  font-size: 1.2rem;
}

.modal-enter-active,
.modal-leave-active {
  transition: opacity 0.3s ease;
}

.modal-enter-from,
.modal-leave-to {
  opacity: 0;
}

.modal-enter-active .modal-container,
.modal-leave-active .modal-container {
  transition: transform 0.3s ease;
}

.modal-enter-from .modal-container,
.modal-leave-to .modal-container {
  transform: scale(0.9) translateY(30px);
}

@media (max-width: 768px) {
  .modal-content {
    padding: 1.5rem 1rem;
  }
  
  .modal-info {
    padding-left: 0;
    margin-top: 1.5rem;
  }
  
  .modal-title {
    font-size: 1.35rem;
  }
  
  .current-price {
    font-size: 1.5rem;
  }
  
  .old-price {
    font-size: 1.1rem;
  }
  
  .modal-main-image {
    height: 300px;
  }
  
  .modal-meta {
    flex-direction: column;
    gap: 0.75rem;
  }
  
  .modal-actions {
    flex-direction: column;
  }
  
  .color-label {
    width: 40px;
    height: 40px;
  }
}

@media (max-width: 576px) {
  .modal-overlay {
    padding: 0.5rem;
  }
  
  .modal-container {
    border-radius: 15px;
    max-height: 95vh;
  }
  
  .modal-content {
    padding: 1rem 0.75rem;
  }
  
  .modal-close {
    width: 32px;
    height: 32px;
    top: 0.75rem;
    right: 0.75rem;
  }
  
  .modal-title {
    font-size: 1.2rem;
  }
  
  .current-price {
    font-size: 1.35rem;
  }
  
  .modal-main-image {
    height: 250px;
    border-radius: 12px;
  }
  
  .modal-thumbnails {
    gap: 0.5rem;
  }
  
  .modal-thumbnail {
    border-radius: 8px;
  }
  
  .color-label {
    width: 36px;
    height: 36px;
  }
  
  .size-label {
    padding: 0.5rem 1rem;
    font-size: 0.9rem;
  }
  
  .btn-add-cart,
  .btn-view-detail {
    padding: 0.875rem;
    font-size: 0.95rem;
  }
}
</style>