import axios from 'axios'

const API_URL = 'http://localhost:5000/api'
const VUE_BASE_URL = 'http://localhost:5173'

export default {
  async getFeaturedProducts() {
    try {
      const response = await axios.get(`${API_URL}/product/featured`)
      const products = response.data.map(product => {
        const mainImage = this.getMainImage(product)
        return {
          ...product,
          mainImage
        }
      })
      return products
    } catch (error) {
      console.error('Error fetching products:', error)
      return []
    }
  },

  async getProducts(params = {}) {
    try {
      const res = await axios.get(`${API_URL}/product`, { params })
      const products = Array.isArray(res.data.products) ? res.data.products : []
      const processedProducts = products.map(product => {
        const mainImage = this.getMainImage(product)
        return {
          ...product,
          mainImage
        }
      })
      return {
        products: processedProducts,
        categories: res.data.categories || [],
        brands: res.data.brands || [],
        totalProducts: res.data.totalProducts || 0,
        totalPages: res.data.totalPages || 1
      }
    } catch (error) {
      console.error('Error fetching products:', error)
      return {
        products: [],
        categories: [],
        brands: [],
        totalProducts: 0,
        totalPages: 1
      }
    }
  },

  async getBrands() {
    const res = await axios.get('/api/Brand')
    return res.data
  },

  async getCategories() {
    const res = await axios.get('/api/Category')
    return res.data
  },

  async getSuggestions(query) {
    const res = await axios.get(`${API_URL}/Product/suggest`, { params: { query } })
    return res.data
  },

  async getProductDetail(slug) {
    try {
      const res = await axios.get(`${API_URL}/Product/slug/${slug}`)
      const product = res.data
      const mainImage = this.getMainImage(product)
      const images = product.hinhAnh?.map(img => 
        `${VUE_BASE_URL}${img.duongDan.replace('/assets', '/src/assets')}`
      )
      return {
        ...product,
        mainImage,
        images,
      }
    } catch (error) {
      console.error('Error fetching product detail:', error)
      throw new Error('Không thể tải thông tin sản phẩm')
    }
  },

  async getProductReviews(maSanPham) {
    try {
      const res = await axios.get(`${API_URL}/ProductReviews/product/${maSanPham}`)
      return res.data
    } catch (error) {
      console.error('Error fetching reviews:', error)
      return []
    }
  },

  async submitReview(reviewData) {
    try {
      const res = await axios.post(`${API_URL}/ProductReviews`, reviewData);
      return res.data;
    } catch (error) {
      console.error('Error submitting review:', error);
      if (error.response) {
        throw new Error(error.response.data.message || 'Gửi đánh giá thất bại');
      }
      throw new Error('Lỗi kết nối server');
    }
  },

  async getRelatedProducts(slug) {
    try {
      const res = await axios.get(`${API_URL}/Product/related/${slug}`);
      return res.data.map(product => {
        const mainImage = this.getMainImage(product);
        return { ...product, mainImage };
      });
    } catch (error) {
      console.error('Error fetching related products:', error);
      return [];
    }
  },

  getMainImage(product) {
    if (!product.hinhAnh || !product.hinhAnh.length) return null
    const mainImage = product.hinhAnh.find(img => img.anhChinh) || product.hinhAnh[0]
    if (!mainImage) return null
    const imagePath = mainImage.duongDan.replace('/assets', '/src/assets')
    return `${VUE_BASE_URL}${imagePath}`
  }
}