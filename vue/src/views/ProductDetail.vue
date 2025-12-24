<template>
  <div class="product-detail">
    <div class="container">
      <div v-if="product">
        <nav aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="/">Trang chủ</a></li>
            <li class="breadcrumb-item"><a href="/san-pham">Sản phẩm</a></li>
            <li class="breadcrumb-item active">{{ product.tenSanPham }}</li>
          </ol>
        </nav>
        <GeneralInfor :product="product" />
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
              <ProductReview :product="product" :reviews="reviews" @review-added="addNewReview" :is-logged-in="isLoggedIn" />
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
    if (product.value) {
      const reviewRes = await ProductService.getProductReviews(product.value.maSanPham)
      reviews.value = reviewRes
      const relatedRes = await ProductService.getRelatedProducts(slugValue)
      relatedProducts.value = relatedRes
    }
  } catch (error) {
    console.error('Error fetching product data:', error)
    product.value = null
  }
}

const addNewReview = async (newReview) => {
  reviews.value.unshift(newReview)
  try {
    const freshReviews = await ProductService.getProductReviews(product.value.id)
    reviews.value = freshReviews
  } catch (error) {
    console.error('Error reloading reviews:', error)
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

</style>