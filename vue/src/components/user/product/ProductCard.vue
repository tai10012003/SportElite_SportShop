<template>
  <div class="product-card">
    <div class="product-badge" v-if="product.giaKhuyenMai && product.giaKhuyenMai < product.gia">
      -{{ Math.round((1 - product.giaKhuyenMai / product.gia) * 100) }}%
    </div>
    <div class="product-image">
      <router-link :to="`/san-pham/` + product.slug">
        <img :src="getImagePath(product)" :alt="product.tenSanPham" class="img-fluid">
      </router-link>
      <div class="product-actions">
        <button class="btn btn-light btn-sm" @click="$emit('add-to-cart', product)">
          <i class="bi bi-cart-plus"></i>
        </button>
        <button class="btn btn-light btn-sm">
          <i class="bi bi-heart"></i>
        </button>
        <router-link :to="`/san-pham/` + product.slug" class="btn btn-light btn-sm">
          <i class="bi bi-eye"></i>
        </router-link>
      </div>
    </div>
    <div class="product-info">
      <router-link :to="`/san-pham/`" class="text-decoration-none">
        <h3 class="product-title">{{ product.tenSanPham }}</h3>
      </router-link>
      <div class="product-price">
        <span class="price-new">{{ formatPrice(product.giaKhuyenMai || product.gia) }}₫</span>
        <span class="price-old" v-if="product.giaKhuyenMai">{{ formatPrice(product.gia) }}₫</span>
      </div>
      <div class="product-rating">
        <i v-for="i in 5" :key="i" :class="['bi', i <= Math.floor(product.averageRating || 0) ? 'bi-star-fill text-warning' : (i - (product.averageRating || 0) <= 0.5 ? 'bi-star-half text-warning' : 'bi-star')]"></i>
        <span class="rating-count">({{ (product.averageRating || 0).toFixed(1) }}/5 - {{ product.totalReviews || 0 }} đánh giá)</span>
      </div>
    </div>
  </div>
</template>

<script setup>
defineProps(['product'])
const formatPrice = (price) => new Intl.NumberFormat('vi-VN').format(price)

const getImagePath = (product) => {
  if (product.mainImage) return product.mainImage
  if (!product.hinhAnh || product.hinhAnh.length === 0) return '/images/products/no-image.jpg'
  const mainImage = product.hinhAnh.find(img => img.anhChinh) || product.hinhAnh[0]
  return mainImage ? `/uploads/products/${mainImage.duongDan}` : '/images/products/no-image.jpg'
}
</script>

<style scoped>
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
  height: 3.1em;
  overflow: hidden;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
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
}

.product-rating .bi {
  margin-left: 4px;
}

.product-rating .rating-count {
  margin-left: 4px;
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

/* Animations */
@keyframes fadeInUp {
  from { opacity: 0; transform: translateY(30px); }
  to { opacity: 1; transform: translateY(0); }
}

.product-card {
  animation: fadeInUp 1s ease-out;
}

/* Media Queries */
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
    font-size: 1rem;
  }
}
</style>