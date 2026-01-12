<template>
  <div class="product-detail">
    <div class="container">
      <div v-if="product">
        <nav aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item">
              <router-link to="/">Trang chủ</router-link>
            </li>
            <li class="breadcrumb-item">
              <router-link to="/san-pham">Sản phẩm</router-link>
            </li>
            <li class="breadcrumb-item active">{{ product.tenSanPham }}</li>
          </ol>
        </nav>
        <GeneralInfor :product="product" :reviews="reviews" />
        <div class="detail-tabs">
          <ul class="nav nav-tabs detail-tab-nav" role="tablist">
            <li class="nav-item"><a class="nav-link active" data-bs-toggle="tab" href="#description">Mô tả chi tiết</a></li>
            <li class="nav-item"><a class="nav-link" data-bs-toggle="tab" href="#specifications">Thông số kỹ thuật</a></li>
            <li class="nav-item"><a class="nav-link" data-bs-toggle="tab" href="#reviews">Đánh giá</a></li>
          </ul>
          <div class="detail-tab-content">
            <div class="tab-content">
              <DetailedDes :product="product" />
              <Specifications :product="product" />
              <ProductReview :product="product" :reviews="reviews" @review-added="addNewReview" @review-updated="reloadReviews" @review-deleted="reloadReviews" :is-logged-in="isLoggedIn" />
            </div>
          </div>
        </div>
        <RelatedProduct :related-products="relatedProducts" />
      </div>
      <div v-else>Loading...</div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue'
import GeneralInfor from '@/components/user/productdetail/GeneralInfor.vue'
import DetailedDes from '@/components/user/productdetail/DetailedDes.vue'
import Specifications from '@/components/user/productdetail/Specifications.vue'
import ProductReview from '@/components/user/productdetail/ProductReview.vue'
import RelatedProduct from '@/components/user/productdetail/RelatedProduct.vue'
import ProductService from '@/services/ProductService'
import AuthService from '@/services/AuthService'

const props = defineProps({
  slug: String
})

const product = ref(null)
const reviews = ref([])
const relatedProducts = ref([])
const isLoggedIn = ref(AuthService.isLoggedIn())

const fetchProductData = async (slugValue) => {
  try {
    const productData = await ProductService.getProductDetail(slugValue)
    product.value = productData
    if (product.value && product.value.maSanPham) {
      const reviewRes = await ProductService.getProductReviews(product.value.maSanPham)
      reviews.value = reviewRes || []
      const relatedRes = await ProductService.getRelatedProducts(slugValue)
      relatedProducts.value = relatedRes || []
    } else {
      reviews.value = []
      relatedProducts.value = []
    }
  } catch (error) {
    console.error('Error fetching product data:', error)
    product.value = null
    reviews.value = []
    relatedProducts.value = []
  }
}

const addNewReview = async (newReview) => {
  reviews.value.unshift(newReview)
  await reloadReviews()
}

const reloadReviews = async () => {
  if (product.value && product.value.maSanPham) {
    try {
      const freshReviews = await ProductService.getProductReviews(product.value.maSanPham)
      reviews.value = freshReviews || []
    } catch (error) {
      console.error('Error reloading reviews:', error)
    }
  }
}

onMounted(() => {
  if (props.slug) fetchProductData(props.slug)
})

watch(() => props.slug, (newSlug) => {
  if (newSlug) fetchProductData(newSlug)
})

</script>

<style scoped>
.product-detail {
  padding: 30px 150px;
  background: var(--light-color);
}

.product-detail .breadcrumb {
  background: transparent;
  padding: 0.5rem 0 1.5rem;
}

.product-detail .breadcrumb-item a {
  color: var(--primary-color);
  font-size: 0.95rem;
}

.product-detail .breadcrumb-item.active {
  color: #666;
}

.detail-tabs {
  background: white;
  border-radius: 15px;
  box-shadow: 0 5px 20px rgba(0, 0, 0, 0.05);
  margin: 2.5rem 0;
}

.detail-tab-nav {
  background: var(--light-color);
  padding: 0;
  border: none;
  position: relative;
}

.nav-item {
  position: relative;
}

.nav-link {
  color: #666;
  font-weight: 500;
  padding: 1rem 2rem;
  border: none !important;
  transition: all 0.3s ease;
  background: transparent;
}

.nav-link:hover {
  transform: translateY(-2px);
  color: var(--primary-color);
  background: rgba(var(--primary-rgb), 0.05);
}

.nav-link::before {
  content: '';
  position: absolute;
  bottom: 0;
  left: 50%;
  width: 0;
  height: 2px;
  background: var(--gradient-hover);
  transition: all 0.3s ease;
  transform: translateX(-50%);
}

.nav-link.active::before {
  width: 100%;
  color: var(--primary-color);
}

.detail-tab-content {
  padding: 1.5rem;
}

@media (max-width: 1200px) {
  .product-detail {
    padding: 25px 80px;
  }
  
  .nav-link {
    padding: 0.9rem 1.5rem;
    font-size: 0.95rem;
  }
}

@media (max-width: 992px) {
  .product-detail {
    padding: 20px 40px;
  }
  
  .detail-tabs {
    margin: 2rem 0;
  }
  
  .nav-link {
    padding: 0.8rem 1.2rem;
    font-size: 0.9rem;
  }
  
  .detail-tab-content {
    padding: 1.2rem;
  }
}

@media (max-width: 768px) {
  .product-detail {
    padding: 15px 20px;
  }
  
  .product-detail .breadcrumb {
    padding: 0.4rem 0 1rem;
    font-size: 0.85rem;
  }
  
  .detail-tabs {
    border-radius: 12px;
    margin: 1.5rem 0;
  }
  
  .detail-tab-nav {
    overflow-x: auto;
    overflow-y: hidden;
    white-space: nowrap;
    -webkit-overflow-scrolling: touch;
    scrollbar-width: none;
    -ms-overflow-style: none;
  }
  
  .detail-tab-nav::-webkit-scrollbar {
    display: none;
  }
  
  .nav-item {
    display: inline-block;
  }
  
  .nav-link {
    padding: 0.75rem 1rem;
    font-size: 0.85rem;
  }
  
  .detail-tab-content {
    padding: 1rem;
  }
}

@media (max-width: 576px) {
  .product-detail {
    padding: 10px 15px;
  }
  
  .product-detail .breadcrumb {
    padding: 0.3rem 0 0.8rem;
    font-size: 0.8rem;
  }
  
  .product-detail .breadcrumb-item a {
    font-size: 0.8rem;
  }
  
  .detail-tabs {
    border-radius: 10px;
    margin: 1rem 0;
  }
  
  .nav-link {
    padding: 0.6rem 0.8rem;
    font-size: 0.8rem;
  }
  
  .detail-tab-content {
    padding: 0.8rem;
  }
}

@media (max-width: 400px) {
  .product-detail {
    padding: 8px 10px;
  }
  
  .nav-link {
    padding: 0.5rem 0.7rem;
    font-size: 0.75rem;
  }
  
  .detail-tab-content {
    padding: 0.6rem;
  }
}
</style>