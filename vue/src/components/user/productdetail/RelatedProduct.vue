<template>
  <div class="related-products">
    <h3 class="section-title">SẢN PHẨM LIÊN QUAN</h3>
    <div class="row g-4">
      <div v-for="related in relatedProducts" :key="related.id" class="col-lg-3 col-md-4 col-sm-6">
        <div class="product-card">
          <div v-if="related.giaKhuyenMai && related.giaKhuyenMai < related.gia" class="product-badge">
            -{{ calculateDiscountPercent(related.gia, related.giaKhuyenMai) }}%
          </div>
          <div class="product-image">
            <a :href="'/san-pham/' + related.slug" class="product-link">
              <img :src="related.mainImage || '/WebbandoTT/app/public/images/no-image.jpg'" :alt="related.tenSanPham" class="img-fluid">
            </a>
            <div class="product-actions">
              <button class="btn btn-light btn-sm add-to-cart" :data-product-id="related.id" :data-product-name="related.tenSanPham" :data-product-price="related.giaKhuyenMai || related.gia">
                <i class="bi bi-cart-plus"></i>
              </button>
              <button class="btn btn-light btn-sm btn-wishlist">
                <i class="bi bi-heart"></i>
              </button>
              <a :href="'/san-pham/' + related.slug" class="btn btn-light btn-sm">
                <i class="bi bi-eye"></i>
              </a>
            </div>
          </div>
          <div class="product-info">
            <a :href="'/san-pham/' + related.slug" class="text-decoration-none">
              <h3 class="product-title">{{ related.tenSanPham }}</h3>
            </a>
            <div class="product-category">
              {{ related.tenDanhMuc || 'Danh mục' }} | {{ related.tenThuongHieu || 'Thương hiệu' }}
            </div>
            <div class="product-price">
              <span class="price-new">{{ formatPrice(related.giaKhuyenMai || related.gia) }}₫</span>
              <span v-if="related.giaKhuyenMai && related.giaKhuyenMai < related.gia" class="price-old">
                {{ formatPrice(related.gia) }}₫
              </span>
            </div>
            <div class="product-rating">
              <i v-for="i in 5" :key="i" :class="['bi', i <= (related.averageRating || 0) ? 'bi-star-fill text-warning' : (i - related.averageRating <= 0.5) ? 'bi-star-half text-warning' : 'bi-star']"></i>
              <span class="rating-count">({{ (related.averageRating || 0).toFixed(1) }} - {{ related.totalReviews || 0 }} đánh giá)</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'

defineProps({
  relatedProducts: {
    type: Array,
    default: () => []
  }
})

const formatPrice = (price) => {
  return new Intl.NumberFormat('vi-VN').format(price)
}

const calculateDiscountPercent = (original, discounted) => {
  if (!original || !discounted || discounted >= original) return 0
  return Math.round(((original - discounted) / original) * 100)
}
</script>

<style scoped>
.related-products {
  margin-top: 2rem;
  padding-top: 2rem;
  border-top: 1px solid var(--light-color);
}

.related-products .section-title {
  font-size: 1.8rem;
  font-weight: 700;
  text-align: center;
  margin-bottom: 2.5rem;
  position: relative;
  padding-bottom: 1rem;
  background: var(--gradient);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}

.related-products .section-title::after {
  content: '';
  position: absolute;
  bottom: 0;
  left: 50%;
  transform: translateX(-50%);
  width: 80px;
  height: 3px;
  background: var(--gradient);
  border-radius: 2px;
}

.product-card {
  border: none;
  border-radius: 20px;
  overflow: hidden;
  transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
  box-shadow: 0 5px 15px rgba(0,0,0,0.08);
  background: rgba(255,255,255,0.95);
  backdrop-filter: blur(10px);
  height: 450px;
  margin-bottom: 30px;
  animation: fadeInUp 1s ease-out;
}

.product-image {
  height: 280px;
  position: relative;
  overflow: hidden;
}

.product-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.5s ease;
}

.product-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 15px 30px rgba(0,0,0,0.15);
}

.product-card:hover img {
  transform: scale(1.1);
}

.product-info {
  padding: 1.25rem;
  display: flex;
  flex-direction: column;
  flex: 1;
}

.product-title {
  font-size: 1.1rem;
  font-weight: 600;
  color: #333;
  margin-bottom: 0.5rem;
  line-height: 1.4;
  overflow: hidden;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
}

.product-category {
  font-size: 0.85rem;
  color: #666;
  margin: 0.5rem 0;
}

.product-price {
  font-weight: 700;
  color: #003780;
  font-size: 1.2rem;
  margin-bottom: 0.5rem;
}

.price-old {
  text-decoration: line-through;
  color: #999;
  font-size: 0.9rem;
  margin-left: 0.5rem;
}

.product-rating {
  color: #ffc107;
  font-size: 0.9rem;
  margin-top: auto;
}

.product-rating .bi {
  margin-right: 2px;
}

.product-rating .rating-count {
  margin-left: 4px;
  color: #666;
  font-size: 0.85rem;
}

.product-badge {
  position: absolute;
  top: 10px;
  left: 10px;
  background: #003780;
  color: white;
  padding: 0.25rem 0.75rem;
  border-radius: 20px;
  font-size: 0.85rem;
  z-index: 1;
  font-weight: 600;
}

.product-actions {
  position: absolute;
  right: 10px;
  top: 50%;
  transform: translateY(-50%);
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  opacity: 0;
  transition: all 0.3s ease;
}

.product-card:hover .product-actions {
  opacity: 1;
  right: 20px;
}

.product-actions .btn {
  width: 40px;
  height: 40px;
  display: flex;
  align-items: center;
  justify-content: center;
  box-shadow: 0 4px 10px rgba(0,0,0,0.1);
}

@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@media (max-width: 768px) {
  .product-card {
    height: 400px;
  }
  .product-image {
    height: 240px;
  }
  .product-title {
    font-size: 1rem;
  }
  .product-price {
    font-size: 1.1rem;
  }
}
</style>