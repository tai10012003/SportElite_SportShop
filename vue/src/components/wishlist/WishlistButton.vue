<template>
    <button 
      class="btn btn-light btn-sm wishlist-btn" 
      :class="{ 'active': isInWishlist }"
      @click.stop="handleToggle"
      :disabled="loading"
      :title="isInWishlist ? 'Bỏ yêu thích' : 'Thêm vào yêu thích'"
    >
      <i 
        class="bi" 
        :class="isInWishlist ? 'bi-heart-fill' : 'bi-heart'"
      ></i>
      <span v-if="loading" class="spinner-border spinner-border-sm ms-1"></span>
    </button>
</template>
  
<script setup>
import { ref, onMounted } from 'vue'
import Swal from 'sweetalert2'
import WishlistService from '@/services/WishlistService'

const props = defineProps({
    maSanPham: {
        type: String,
        required: true
    }
})

const emit = defineEmits(['toggle'])

const isInWishlist = ref(false)
const loading = ref(false)

const checkWishlist = async () => {
    try {
        const result = await WishlistService.checkProduct(props.maSanPham)
        isInWishlist.value = result.isInWishlist
    } catch (error) {
        console.error('Error checking wishlist:', error)
    }
}

const handleToggle = async () => {
    loading.value = true
    try {
        const result = await WishlistService.toggleWishlist(props.maSanPham)
        if (result.success) {
            isInWishlist.value = result.isInWishlist
            if (result.isInWishlist) {
                Swal.fire({
                icon: 'success',
                title: 'Thành công!',
                text: result.message || 'Đã thêm vào yêu thích',
                timer: 2000,
                showConfirmButton: false,
                toast: true,
                position: 'top-end'
                })
            } else {
                Swal.fire({
                icon: 'info',
                title: 'Đã xóa',
                text: result.message || 'Đã xóa khỏi yêu thích',
                timer: 2000,
                showConfirmButton: false,
                toast: true,
                position: 'top-end'
                })
            }
            emit('toggle', { maSanPham: props.maSanPham, isInWishlist: result.isInWishlist })
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Lỗi!',
                text: result.message || 'Có lỗi xảy ra',
                timer: 2000,
                showConfirmButton: false,
                toast: true,
                position: 'top-end'
            })
        }
    } catch (error) {
        Swal.fire({
        icon: 'error',
        title: 'Lỗi!',
        text: 'Không thể cập nhật danh sách yêu thích',
        timer: 2000,
        showConfirmButton: false,
        toast: true,
        position: 'top-end'
        })
    } finally {
        loading.value = false
    }
}

onMounted(() => {
    checkWishlist()
})
</script>
  
<style scoped>
.wishlist-btn {
    position: relative;
    transition: all 0.3s ease;
    width: 35px;
    height: 33px;
    display: flex;
    align-items: center;
    justify-content: center;
    border-radius: 5px;
    background: rgba(255, 255, 255, 0.9);
    border: 1px solid #e0e0e0;
}

.wishlist-btn:hover {
    background: #fff;
}

.wishlist-btn.active i {
    color: #ff4757;
    animation: heartBeat 0.6s ease;
}

.wishlist-btn:disabled {
    opacity: 0.6;
    cursor: not-allowed;
}

@keyframes heartBeat {
    0%, 100% { transform: scale(1); }
    25% { transform: scale(1.3); }
    50% { transform: scale(1.1); }
    75% { transform: scale(1.25); }
}
</style>