import axios from 'axios'

const API_URL = 'http://localhost:5000/api/wishlist'

class WishlistService {
    async toggleWishlist(maSanPham) {
        try {
            const response = await axios.post(`${API_URL}/toggle`, { maSanPham })
            return response.data
        } catch (error) {
            console.error('Error toggling wishlist:', error)
            throw error
        }
    }

    async getWishlist() {
        try {
            const response = await axios.get(API_URL)
            return response.data
        } catch (error) {
            console.error('Error getting wishlist:', error)
            throw error
        }
    }

    async addToWishlist(maSanPham) {
        try {
            const response = await axios.post(API_URL, { maSanPham })
            return response.data
        } catch (error) {
            console.error('Error adding to wishlist:', error)
            throw error
        }
    }

    async removeFromWishlist(maSanPham) {
        try {
            const response = await axios.delete(`${API_URL}/${maSanPham}`)
            return response.data
        } catch (error) {
            console.error('Error removing from wishlist:', error)
            throw error
        }
    }

    async checkProduct(maSanPham) {
        try {
            const response = await axios.get(`${API_URL}/check/${maSanPham}`)
            return response.data
        } catch (error) {
            console.error('Error checking product:', error)
            throw error
        }
    }

    async checkMultipleProducts(maSanPhams) {
        try {
            const response = await axios.post(`${API_URL}/check-multiple`, maSanPhams)
            return response.data
        } catch (error) {
            console.error('Error checking multiple products:', error)
            throw error
        }
    }
}

export default new WishlistService()