<script setup>
import { ref, onMounted } from 'vue'
import { addToCart } from '@/assets/js/main'
import ProductService from '@/services/ProductService'

const featuredProducts = ref([])

onMounted(async () => {
  try {
    const products = await ProductService.getFeaturedProducts()
    featuredProducts.value = products.map(p => ({
      ...p,
      avg_rating: p.avg_rating || 0,
      total_reviews: p.total_reviews || 0
    }))
  } catch (error) {
    console.error('Error fetching products:', error)
  }
})

const formatPrice = (price) => {
  return new Intl.NumberFormat('vi-VN').format(price)
}

const handleAddToCart = async (product) => {
  const cartCount = await addToCart(product.id)
  if (cartCount) {
    console.log('Cart updated, new count:', cartCount)
  }
}
</script>

<template>
  <section class="featured-products bg-light">
    <div class="container">
      <h2 class="section-title">SẢN PHẨM NỔI BẬT</h2>
      <div class="row g-4">
        <div v-for="product in featuredProducts" :key="product.id" class="col-lg-3 col-md-6">
          <div class="product-card">
            <div class="product-badge" v-if="product.giaKhuyenMai">
              -{{ Math.round((1 - product.giaKhuyenMai/product.gia) * 100) }}%
            </div>
            <div class="product-image" v-if="product.mainImage">
              <router-link :to="'/san-pham/' + product.slug">
                <img :src="product.mainImage" :alt="product.tenSanPham">
              </router-link>
              <div class="product-actions">
                <button class="btn btn-light btn-sm" @click="handleAddToCart(product)">
                  <i class="bi bi-cart-plus"></i>
                </button>
                <button class="btn btn-light btn-sm">
                  <i class="bi bi-heart"></i>
                </button>
                <router-link :to="'/san-pham/' + product.slug" class="btn btn-light btn-sm">
                  <i class="bi bi-eye"></i>
                </router-link>
              </div>
            </div>
            <div class="product-info">
              <router-link :to="'/san-pham/' + product.slug" class="text-decoration-none">
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
        </div>
      </div>
    </div>
  </section>
</template>

<style scoped>
.featured-products {
  padding: 70px 0;
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

@keyframes fadeInUp {
  from { opacity: 0; transform: translateY(30px); }
  to { opacity: 1; transform: translateY(0); }
}

.product-card {
  animation: fadeInUp 1s ease-out;
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
    font-size: 1rem;
  }
}
</style>