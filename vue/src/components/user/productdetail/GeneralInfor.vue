<template>
  <div class="row">
      <div class="col-lg-5">
        <div class="detail-gallery">
          <div class="detail-main-image">
            <img :src="mainImage" alt="Main Product Image" id="main-product-image">
          </div>
          <div class="detail-thumbnails-wrapper">
            <button class="thumbnail-nav prev" @click="slideThumbnails('prev')" :disabled="currentSlide === 0">
              <i class="bi bi-chevron-left"></i>
            </button>
            <div class="detail-thumbnails" ref="thumbnailContainer">
              <img v-for="(img, index) in product.images" 
                  :key="index" 
                  :src="img" 
                  :alt="'Thumbnail ' + (index + 1)"
                  class="detail-thumbnail" 
                  :class="{ 'active': currentImageIndex == index }" 
                  @click="changeMainImage(img, index)">
            </div>
            <button class="thumbnail-nav next" @click="slideThumbnails('next')" :disabled="currentSlide >= Math.ceil(product.images.length / 4) - 1">
              <i class="bi bi-chevron-right"></i>
            </button>
          </div>
        </div>
      </div>
      <div class="col-lg-7">
        <div class="detail-info-wrapper">
          <h1 class="detail-product-title">{{ product.tenSanPham }}</h1>

          <div class="detail-meta-info">
            <div class="detail-meta-item">
              <span class="detail-meta-label">Mã sản phẩm:</span>
              <span>{{ product.maSanPham }}</span>
            </div>
            <div class="detail-meta-item">
              <span class="detail-meta-label">Danh mục:</span>
              <span>{{ product.tenDanhMuc }}</span>
            </div>
            <div class="detail-meta-item">
              <span class="detail-meta-label">Thương hiệu:</span>
              <span>{{ product.tenThuongHieu }}</span>
            </div>
            <div class="detail-meta-item">
              <span class="detail-meta-label">Lượt xem:</span>
              <span>{{ product.luotXem }}</span>
            </div>
            <div class="detail-meta-item">
              <span class="detail-meta-label">Đánh giá:</span>
              <span class="rating-wrapper">
                <div class="star-rating">
                  <i v-for="i in 5" 
                    :key="i" 
                    :class="[
                      'bi', 
                      i <= averageRating 
                        ? 'bi-star-fill' 
                        : (i - averageRating <= 0.5 
                          ? 'bi-star-half' 
                          : 'bi-star'),
                      'star-icon'
                    ]">
                  </i>
                </div>
                <span class="rating-text">
                  ({{ averageRating.toFixed(1) }}/5 - {{ totalReviews }} đánh giá)
                </span>
              </span>
            </div>
          </div>
          <div class="detail-price-wrapper">
            <label class="form-label">Giá: </label>
            <span class="detail-current-price">{{ formatPrice(product.giaKhuyenMai || product.gia) }}₫</span>
            <span class="detail-old-price" v-if="product.giaKhuyenMai">{{ formatPrice(product.gia) }}₫</span>
            <span class="detail-discount" v-if="product.giaKhuyenMai">
              -{{ Math.round(((product.gia - product.giaKhuyenMai) / product.gia) * 100) }}%
            </span>
          </div>
          <form v-if="product.tinhTrang && product.soLuong > 0" @submit.prevent="addToCart" class="detail-add-cart-form">
            <input type="hidden" name="product_id" :value="product.id">
            <div class="detail-quantity-control">
              <label class="detail-quantity-label">Số lượng:</label>
              <div class="input-group">
                <button type="button" class="btn btn-outline-secondary" @click="updateQuantity('decrease')">-</button>
                <input type="number" class="form-control text-center" v-model="quantity" min="1" :max="product.soLuong">
                <button type="button" class="btn btn-outline-secondary" @click="updateQuantity('increase')">+</button>
              </div>
              <span class="detail-stock-info">(Còn {{ product.soLuong }} sản phẩm)</span>
            </div>
            <div class="product-variations mt-4 mb-4" v-if="product.mauSac || product.kichThuoc">
              <div class="variation-group mb-4" v-if="product.mauSac">
                <label class="form-label">Màu sắc:</label>
                <div class="color-options">
                  <div v-for="(color, index) in getColors" :key="index" class="color-option">
                    <input type="radio" 
                          :id="'color_' + color.trim()" 
                          :value="color.trim()"
                          v-model="selectedColor" 
                          class="color-radio"
                          required>
                    <label :for="'color_' + color.trim()" 
                          class="color-label"
                          :style="getColorStyle(color.trim())"
                          :title="color.trim()">
                      <span class="color-name">{{ color.trim() }}</span>
                    </label>
                  </div>
                </div>
              </div>
              <div class="variation-group" v-if="product.kichThuoc">
                <label class="form-labels">Kích thước:</label>
                <div class="size-options">
                  <div v-for="(size, index) in getSizes" :key="index" class="size-option">
                    <input type="radio" 
                          :id="'size_' + size.trim()" 
                          :value="size.trim()"
                          v-model="selectedSize" 
                          class="size-radio"
                          required>
                    <label :for="'size_' + size.trim()" class="size-label">
                      {{ size.trim() }}
                    </label>
                  </div>
                </div>
              </div>
            </div>
            <div class="detail-buttons">
              <button type="submit" class="detail-add-to-cart">
                <i class="bi bi-cart-plus"></i> Thêm vào giỏ hàng
              </button>
              <button type="button" class="detail-add-to-wishlist">
                <i class="bi bi-heart"></i> Thêm vào yêu thích
              </button>
            </div>
          </form>
          <div v-else class="alert alert-warning mt-3" role="alert">
            <i class="bi bi-exclamation-triangle-fill me-2"></i>
            <strong>{{ !product.tinhTrang ? 'Sản phẩm tạm ngừng kinh doanh!' : 'Sản phẩm đã hết hàng!' }}</strong>
            <p class="mb-0 mt-2">Vui lòng quay lại sau hoặc liên hệ với chúng tôi để biết thêm thông tin!</p>
          </div>
          <div class="detail-social-share">
            <a href="#" class="detail-social-btn detail-facebook"><i class="bi bi-facebook"></i></a>
            <a href="#" class="detail-social-btn detail-twitter"><i class="bi bi-twitter"></i></a>
            <a href="#" class="detail-social-btn detail-pinterest"><i class="bi bi-pinterest"></i></a>
          </div>
          <div class="detail-short-desc">
            <h5 class="detail-short-desc-title">Mô tả sản phẩm</h5>
            <div class="detail-short-desc-content">
              <span v-if="product.moTa && product.moTa.trim() !== ''">{{ product.moTa }}</span>
              <span v-else class="no-description">
                Hiện tại chưa có mô tả cho sản phẩm này. Chúng tôi sẽ cập nhật sớm nhất !!
              </span>
            </div>
          </div>
        </div>
      </div>
    </div>
</template>

<script setup>
import { ref, onMounted, computed  } from 'vue'
import { getColorStyle } from '@/assets/js/colorMap'

const props = defineProps({
  product: {
    type: Object,
    required: true
  },
  reviews: {
    type: Array,
    default: () => []
  }
})

const product = props.product
const mainImage = ref('')
const quantity = ref(1)
const selectedColor = ref('')
const selectedSize = ref('')
const currentImageIndex = ref(0)
const currentSlide = ref(0)
const thumbnailContainer = ref(null)

const changeMainImage = (src, index) => {
  mainImage.value = src
  currentImageIndex.value = index
}

const slideThumbnails = (direction) => {
  if (direction == 'next' && currentSlide.value < Math.ceil(product.images.length / 4) - 1) {
    currentSlide.value++
  } else if (direction == 'prev' && currentSlide.value > 0) {
    currentSlide.value--
  }
  
  const offset = currentSlide.value * 100
  thumbnailContainer.value.style.transform = `translateX(-${offset}%)`
}

const formatPrice = (price) => new Intl.NumberFormat('vi-VN').format(price)
const updateQuantity = (action) => {
  const max = parseInt(product.soLuong)
  if (action == 'increase' && quantity.value < max) quantity.value++
  else if (action == 'decrease' && quantity.value > 1) quantity.value--
}

const addToCart = () => {
  console.log('Adding to cart:', {
    productId: product.id,
    quantity: quantity.value,
    color: selectedColor.value,
    size: selectedSize.value
  })
  alert('Đã thêm vào giỏ hàng: ' + product.tenSanPham)
}
const getColors = computed(() => {
  return product.mauSac ? product.mauSac.split(',').map(c => c.trim()) : []
})

const getSizes = computed(() => {
  return product.kichThuoc ? product.kichThuoc.split(',').map(s => s.trim()) : []
})

const averageRating = computed(() => {
  if (!props.reviews || props.reviews.length === 0) return 0
  const sum = props.reviews.reduce((acc, review) => acc + review.diemDanhGia, 0)
  const avg = sum / props.reviews.length
  return Math.round(avg * 10) / 10
})

const totalReviews = computed(() => {
  return props.reviews?.length || 0
})

onMounted(() => {
  mainImage.value = product.images?.[0] || '/WebbandoTT/app/public/images/no-image.jpg'
})
</script>

<style scoped>
.detail-gallery {
  background: white;
  border-radius: 15px;
  box-shadow: 0 8px 25px rgba(0, 0, 0, 0.08);
  position: sticky;
  top: 90px;
  transition: all 0.3s ease;
  padding: 1rem;
}

.detail-main-image {
  border-radius: 12px;
  overflow: hidden;
  margin-bottom: 1rem;
  background: #f8f9fa;
  text-align: center;
  height: 500px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.detail-main-image img {
  width: 100%;
  height: 100%;
  transition: transform 0.4s ease;
}

.detail-main-image:hover img {
  transform: scale(1.02);
}

.detail-thumbnails-wrapper {
  position: relative;
  padding: 0 40px;
  margin-top: 20px;
}

.detail-thumbnails {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 12px;
  transition: transform 0.3s ease;
}

.thumbnail-nav {
  position: absolute;
  top: 50%;
  transform: translateY(-50%);
  width: 30px;
  height: 30px;
  border-radius: 50%;
  background: white;
  border: 1px solid #d3d3d3;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: all 0.3s ease;
  z-index: 2;
}

.thumbnail-nav.prev {
  left: 0;
}

.thumbnail-nav.next {
  right: 0;
}

.thumbnail-nav:hover {
  background: var(--primary-color);
  border-color: var(--primary-color);
  color: white;
  box-shadow: 0 2px 8px rgba(0,0,0,0.1);
}

.detail-thumbnail {
  width: 100%;
  aspect-ratio: 1;
  border-radius: 8px;
  cursor: pointer;
  padding: 5px;
  background: white;
  border: 2px solid #eee;
  transition: all 0.3s ease;
  opacity: 0.7;
}

.detail-thumbnail img {
  width: 100%;
  height: 100%;
  object-fit: contain;
  border-radius: 6px;
}

.detail-thumbnail:hover {
  border-color: var(--primary-color);
  transform: translateY(-2px);
  opacity: 1;
}

.detail-thumbnail.active {
  border-color: var(--primary-color);
  box-shadow: 0 3px 10px rgba(0,55,128,0.15);
  transform: translateY(-2px);
  opacity: 1;
}

.detail-info-wrapper {
  background: white;
  border-radius: 15px;
  padding: 1.5rem;
  height: 100%;
  box-shadow: 0 3px 10px rgba(0,0,0,0.05);
}

.detail-product-title {
  font-size: 24px;
  font-weight: 600;
  color: #2c3e50;
  border-bottom: 2px solid #f5f5f5;
  padding-bottom: 1rem;
}

.detail-meta-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.8rem 1rem;
  border-bottom: 1px solid rgba(0,0,0,0.05);
  transition: background-color 0.3s ease;
}

.detail-meta-item:last-child {
  border-bottom: none;
}

.detail-meta-item:hover {
  background-color: #fff;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.02);
}

.detail-meta-label {
  font-weight: 500;
  color: var(--dark-color);
}

.detail-quantity-control {
  display: flex;
  gap: 1.5rem;
  border-radius: 12px;
  margin: 2rem 0;
}

.detail-quantity-label {
  font-weight: 500;
  color: #2c3e50;
  margin-top: 5px;
  display: block;
}

.input-group {
  max-width: 150px;
}

.input-group .btn {
  background: #fff;
  border: 1px solid #dee2e6;
  color: #2c3e50;
  padding: 0.3rem 1.2rem;
  font-weight: 600;
  transition: all 0.3s ease;
}

.input-group .btn:hover {
  background: #f8f9fa;
  color: #0d6efd;
}

.input-group .form-control {
  border: 1px solid #dee2e6;
  font-weight: 600;
  color: #2c3e50;
  padding: 0.5rem;
}

.detail-stock-info {
  display: inline-block;
  padding: 10px 12px;
  color: #666;
  font-size: 0.9rem;
  background: #e8f4ff;
  border-radius: 20px;
  font-weight: 500;
}

@keyframes quantityPulse {
  0% { transform: scale(1); }
  50% { transform: scale(1.05); }
  100% { transform: scale(1); }
}

.form-control:focus {
  box-shadow: none;
  border-color: #0d6efd;
  animation: quantityPulse 0.3s ease-in-out;
}

.detail-price-wrapper {
  margin: 20px 0 10px 0;
  display: flex;
  gap: 1.5rem;
  flex-wrap: wrap;
  align-items: center;
}

.detail-price-wrapper .form-label {
  margin-top: 2px;
  font-weight: 500;
  color: var(--dark-color);
}

.detail-current-price {
  font-size: 20px;
  font-weight: 600;
  color: red;
}

.detail-old-price {
  font-size: 1.2rem;
  text-decoration: line-through;
  color: #999;
}

.detail-discount {
  background: #dc3545;
  color: white;
  padding: 0.5rem 1rem;
  border-radius: 15px;
  font-size: 0.85rem;
  font-weight: 500;
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

.color-radio,
.size-radio {
  display: none;
}

.variation-group {
  display: flex;
  gap: 2rem;
}

.variation-group .form-label {
  margin-top: 15px;
  font-weight: 500;
  color: var(--dark-color);
}

.variation-group .form-labels {
  margin-top: 20px;
  font-weight: 500;
  color: var(--dark-color);
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
  bottom: -25px;
  left: 50%;
  transform: translateX(-50%);
  font-size: 12px;
  white-space: nowrap;
  color: #666;
  opacity: 0;
  transition: opacity 0.3s ease;
}

.color-label:hover .color-name {
  opacity: 1;
}

.color-radio:checked + .color-label {
  transform: scale(1.1);
  box-shadow: 0 0 0 2px #fff, 0 0 0 4px var(--primary-color);
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
  border-color: var(--primary-color);
  color: var(--primary-color);
}

.size-radio:checked + .size-label {
  background: var(--primary-color);
  border-color: var(--primary-color);
  color: white;
  transform: translateY(-2px);
  box-shadow: 0 4px 10px rgba(0,0,0,0.1);
}

.detail-buttons {
  display: flex;
  gap: 1rem;
  margin: 1rem 0;
}

.detail-add-to-cart,
.detail-add-to-wishlist {
  width: 100%;
  padding: 8px;
  font-size: 1rem;
  margin: 1rem 0;
  border: none;
  border-radius: 12px;
  color: white;
  font-weight: 500;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.75rem;
  transition: all 0.3s ease;
}

.detail-add-to-cart {
  background: var(--gradient);
  color: white;
}

.detail-add-to-wishlist {
  background: white;
  color: var(--primary-color);
  border: 2px solid var(--primary-color);
}

.detail-add-to-cart:hover,
.detail-add-to-wishlist:hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 20px rgba(0,0,0,0.15);
}

.detail-add-to-wishlist:hover {
  background: var(--primary-color);
  color: white;
}

.detail-add-to-cart i,
.detail-add-to-wishlist i {
  font-size: 1.2rem;
}

.alert {
  border: none;
  background: white;
}

.alert-success {
  background: #d4edda;
  color: #155724;
  border-left: 4px solid #28a745;
}

.alert-danger {
  background: #f8d7da;
  color: #721c24;
  border-left: 4px solid #dc3545;
}

.detail-social-share {
  margin-top: 1.5rem;
  display: flex;
  gap: 0.75rem;
}

.detail-social-btn {
  width: 36px;
  height: 36px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 1rem;
  transition: all 0.2s ease;
}

.detail-social-btn:hover {
  transform: translateY(-3px);
  opacity: 0.9;
}

.detail-facebook { background: #1877f2; }
.detail-twitter { background: #1da1f2; }
.detail-pinterest { background: #bd081c; }

.detail-short-desc {
  margin-top: 1.5rem;
  padding-top: 1.5rem;
  border-top: 1px solid #dedede;
}

.detail-short-desc-title {
  font-size: 1.1rem;
  font-weight: 600;
  color: var(--dark-color);
  margin-bottom: 1rem;
  position: relative;
  padding-bottom: 0.75rem;
}

.detail-short-desc-title::after {
  content: '';
  position: absolute;
  left: 0;
  bottom: 0;
  width: 40px;
  height: 2px;
  background: var(--gradient);
  border-radius: 2px;
}

.detail-short-desc-content {
  color: #555;
  line-height: 1.8;
  font-size: 0.95rem;
  text-align: justify;
}

.no-description {
  display: block;
  padding: 10px 15px;
  background-color: #fff3cd;
  color: #856404;
  border-radius: 8px;
  font-weight: 500;
  line-height: 1.5;
  margin-top: 8px;
}

@media (max-width: 1200px) {
  .detail-gallery {
    top: 80px;
  }
  
  .detail-main-image {
    height: 450px;
  }
  
  .detail-product-title {
    font-size: 22px;
  }
  
  .detail-current-price {
    font-size: 18px;
  }
}

@media (max-width: 992px) {
  .detail-gallery {
    position: relative;
    top: 0;
    margin-bottom: 2rem;
  }
  
  .detail-main-image {
    height: 400px;
  }
  
  .detail-thumbnails {
    grid-template-columns: repeat(4, 1fr);
    gap: 10px;
  }
  
  .detail-info-wrapper {
    padding: 1.2rem;
  }
  
  .detail-product-title {
    font-size: 20px;
  }
  
  .detail-meta-item {
    padding: 0.7rem 0.8rem;
    font-size: 0.9rem;
  }
  
  .detail-current-price {
    font-size: 18px;
  }
  
  .detail-old-price {
    font-size: 1.1rem;
  }
  
  .variation-group {
    flex-direction: column;
    gap: 1rem;
  }
  
  .variation-group .form-label,
  .variation-group .form-labels {
    margin-top: 0;
  }
}

@media (max-width: 768px) {
  .detail-gallery {
    border-radius: 12px;
    padding: 0.8rem;
  }
  
  .detail-main-image {
    height: 350px;
    border-radius: 10px;
  }
  
  .detail-thumbnails-wrapper {
    padding: 0 35px;
    margin-top: 15px;
  }
  
  .detail-thumbnails {
    grid-template-columns: repeat(3, 1fr);
    gap: 8px;
  }
  
  .thumbnail-nav {
    width: 28px;
    height: 28px;
    font-size: 0.9rem;
  }
  
  .detail-info-wrapper {
    padding: 1rem;
    border-radius: 12px;
  }
  
  .detail-product-title {
    font-size: 18px;
    padding-bottom: 0.8rem;
  }
  
  .detail-meta-item {
    padding: 0.6rem 0.5rem;
    font-size: 0.85rem;
    flex-wrap: wrap;
  }
  
  .rating-wrapper {
    padding: 3px 6px;
  }
  
  .star-icon {
    font-size: 14px;
  }
  
  .rating-text {
    font-size: 12px;
  }
  
  .detail-price-wrapper {
    margin: 15px 0 8px 0;
    gap: 1rem;
  }
  
  .detail-current-price {
    font-size: 16px;
  }
  
  .detail-old-price {
    font-size: 1rem;
  }
  
  .detail-discount {
    padding: 0.4rem 0.8rem;
    font-size: 0.8rem;
  }
  
  .detail-quantity-control {
    flex-direction: column;
    gap: 1rem;
    margin: 1.5rem 0;
  }
  
  .input-group {
    max-width: 100%;
  }
  
  .detail-stock-info {
    padding: 8px 10px;
    font-size: 0.85rem;
  }
  
  .color-options,
  .size-options {
    gap: 15px;
  }
  
  .color-label {
    width: 35px;
    height: 35px;
  }
  
  .size-label {
    padding: 7px 11px;
    font-size: 0.9rem;
  }
  
  .detail-buttons {
    flex-direction: column;
    gap: 0.8rem;
  }
  
  .detail-add-to-cart,
  .detail-add-to-wishlist {
    padding: 10px;
    font-size: 0.95rem;
  }
  
  .detail-social-share {
    gap: 0.6rem;
  }
  
  .detail-social-btn {
    width: 32px;
    height: 32px;
    font-size: 0.9rem;
  }
  
  .detail-short-desc-title {
    font-size: 1rem;
  }
  
  .detail-short-desc-content {
    font-size: 0.9rem;
    line-height: 1.6;
  }
}

@media (max-width: 576px) {
  .detail-gallery {
    border-radius: 10px;
    padding: 0.6rem;
  }
  
  .detail-main-image {
    height: 280px;
    border-radius: 8px;
  }
  
  .detail-thumbnails-wrapper {
    padding: 0 30px;
    margin-top: 12px;
  }
  
  .detail-thumbnails {
    grid-template-columns: repeat(2, 1fr);
    gap: 6px;
  }
  
  .thumbnail-nav {
    width: 26px;
    height: 26px;
    font-size: 0.85rem;
  }
  
  .detail-info-wrapper {
    padding: 0.8rem;
    border-radius: 10px;
  }
  
  .detail-product-title {
    font-size: 16px;
    padding-bottom: 0.6rem;
  }
  
  .detail-meta-item {
    padding: 0.5rem 0.3rem;
    font-size: 0.8rem;
  }
  
  .detail-meta-label {
    font-size: 0.8rem;
  }
  
  .rating-wrapper {
    padding: 2px 5px;
    flex-wrap: wrap;
  }
  
  .star-icon {
    font-size: 12px;
  }
  
  .rating-text {
    font-size: 11px;
  }
  
  .detail-price-wrapper {
    gap: 0.8rem;
  }
  
  .detail-current-price {
    font-size: 15px;
  }
  
  .detail-old-price {
    font-size: 0.95rem;
  }
  
  .detail-discount {
    padding: 0.3rem 0.7rem;
    font-size: 0.75rem;
  }
  
  .detail-quantity-control {
    margin: 1rem 0;
  }
  
  .detail-stock-info {
    padding: 6px 8px;
    font-size: 0.8rem;
  }
  
  .color-label {
    width: 32px;
    height: 32px;
  }
  
  .color-name {
    font-size: 11px;
    bottom: -22px;
  }
  
  .size-label {
    padding: 6px 10px;
    font-size: 0.85rem;
  }
  
  .detail-add-to-cart,
  .detail-add-to-wishlist {
    padding: 8px;
    font-size: 0.9rem;
    gap: 0.5rem;
  }
  
  .detail-add-to-cart i,
  .detail-add-to-wishlist i {
    font-size: 1rem;
  }
  
  .alert {
    font-size: 0.85rem;
    padding: 0.8rem;
  }
  
  .detail-social-share {
    gap: 0.5rem;
    margin-top: 1rem;
  }
  
  .detail-social-btn {
    width: 30px;
    height: 30px;
    font-size: 0.85rem;
  }
  
  .detail-short-desc {
    margin-top: 1rem;
    padding-top: 1rem;
  }
  
  .detail-short-desc-title {
    font-size: 0.95rem;
    margin-bottom: 0.8rem;
  }
  
  .detail-short-desc-content {
    font-size: 0.85rem;
    line-height: 1.5;
  }
  
  .no-description {
    padding: 8px 12px;
    font-size: 0.85rem;
  }
}

@media (max-width: 400px) {
  .detail-main-image {
    height: 240px;
  }
  
  .detail-product-title {
    font-size: 15px;
  }
  
  .detail-current-price {
    font-size: 14px;
  }
  
  .detail-old-price {
    font-size: 0.9rem;
  }
  
  .color-label {
    width: 28px;
    height: 28px;
  }
  
  .size-label {
    padding: 5px 8px;
    font-size: 0.8rem;
  }
}

@media (max-height: 500px) and (orientation: landscape) {
  .detail-gallery {
    position: relative;
  }
  
  .detail-main-image {
    height: 250px;
  }
}
</style>