import axios from 'axios'
import AuthService from './AuthService'
const API_URL = 'http://localhost:5000/api'

axios.interceptors.request.use(
    config => {
        const user = AuthService.getCurrentUser()
        if (user?.token) {
            config.headers['Authorization'] = `Bearer ${user.token}`
        }
        config.headers['Content-Type'] = 'application/json'
        return config
    },
    error => Promise.reject(error)
)

export default {
    async getAllOrders() {
        try {
            const response = await axios.get(`${API_URL}/Order`)
            return response.data
        } catch (error) {
            throw this.handleError(error, 'Không thể tải danh sách đơn hàng')
        }
    },

    async getMyOrders() {
        try {
            const response = await axios.get(`${API_URL}/Order/my-orders`)
            return response.data
        } catch (error) {
            throw this.handleError(error, 'Không thể tải đơn hàng của bạn')
        }
    },

    async getOrderById(id) {
        try {
            const response = await axios.get(`${API_URL}/Order/${id}`)
            return response.data
        } catch (error) {
            throw this.handleError(error, 'Không tìm thấy đơn hàng')
        }
    },

    async getOrderByCode(maDonHang) {
        try {
            const response = await axios.get(`${API_URL}/Order/code/${maDonHang}`)
            return response.data
        } catch (error) {
            throw this.handleError(error, 'Không tìm thấy đơn hàng')
        }
    },

    async createOrder(orderData) {
        try {
            const response = await axios.post(`${API_URL}/Order`, orderData)
            return response.data
        } catch (error) {
            console.error('Create order error:', error.response?.data)
            throw this.handleError(error, 'Tạo đơn hàng thất bại')
        }
    },

    async updateOrderStatus(id, statusData) {
        try {
            const response = await axios.put(`${API_URL}/Order/${id}/status`, statusData)
            return response.data
        } catch (error) {
            throw this.handleError(error, 'Cập nhật trạng thái thất bại')
        }
    },

    async cancelOrder(id) {
        try {
            const response = await axios.put(`${API_URL}/Order/${id}/cancel`)
            return response.data
        } catch (error) {
            throw this.handleError(error, 'Không thể hủy đơn hàng')
        }
    },

    async deleteOrder(id) {
        try {
            const response = await axios.delete(`${API_URL}/Order/${id}`)
            return response.data
        } catch (error) {
            throw this.handleError(error, 'Xóa đơn hàng thất bại')
        }
    },

    handleError(error, defaultMessage) {
        if (error.response?.status === 401) {
            return { message: 'Vui lòng đăng nhập lại' }
        }
        if (error.response?.data?.message) {
            return { message: error.response.data.message }
        }
        if (error.response?.data?.error) {
            return { message: error.response.data.error }
        }
        return { message: defaultMessage }
    }
}