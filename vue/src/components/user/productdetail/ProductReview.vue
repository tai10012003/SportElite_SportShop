<template>
  <div class="tab-pane fade" id="reviews" role="tabpanel">
    <div class="product-reviews">
      <div v-if="!isLoggedIn" class="alert alert-info">
        Vui lòng <router-link to="/dang-nhap">đăng nhập</router-link> để đánh giá sản phẩm
      </div>
      <div v-else>
        <div class="review-form">
          <div class="mb-3">
            <label class="form-label">Đánh giá của bạn</label>
            <div class="rating-input">
              <input type="radio" id="star5" value="5" v-model="newReview.rating">
              <label for="star5" title="Rất tốt">★</label>
              <input type="radio" id="star4" value="4" v-model="newReview.rating">
              <label for="star4" title="Tốt">★</label>
              <input type="radio" id="star3" value="3" v-model="newReview.rating">
              <label for="star3" title="Bình thường">★</label>
              <input type="radio" id="star2" value="2" v-model="newReview.rating">
              <label for="star2" title="Kém">★</label>
              <input type="radio" id="star1" value="1" v-model="newReview.rating">
              <label for="star1" title="Rất kém">★</label>
            </div>
          </div>                      
          <div class="mb-3">
            <label class="form-label">Nhận xét của bạn</label>
            <textarea class="form-control" v-model="newReview.comment" placeholder="Nhận xét của bạn..." rows="3"></textarea>
          </div>
          <button class="btn-submit" @click="submitReview" :disabled="isSubmitting">
            {{ isSubmitting ? 'Đang gửi...' : 'Gửi đánh giá' }}
          </button>
        </div>
        <div class="text-center p-4" v-if="reviews.length === 0">
          <div class="mb-3">
            <i class="bi bi-chat-square-text" style="font-size: 2rem;"></i>
          </div>
          <p class="text-muted">Chưa có đánh giá nào cho sản phẩm này</p>
          <p>Hãy là người đầu tiên đánh giá sản phẩm!</p>
        </div>
        <div v-else class="review-list">
          <div v-for="review in reviews" :key="review.id" class="review-item">
            <div class="review-header">
              <div class="review-author-date">
                <span class="review-author">{{ review.hoTen || "Người dùng" }}</span>
                <span class="review-date">{{ formatDate(review.ngayTao) }}</span>
              </div>
              <div class="review-rating">
                <i v-for="star in 5" :key="star" class="bi" :class="star <= review.soSao ? 'bi-star-fill' : 'bi-star'"></i>
              </div>
            </div>
            <div class="review-content">{{ review.noiDung }}</div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, watch } from 'vue'
import AuthService from '@/services/AuthService'
import ProductService from '@/services/ProductService'

const props = defineProps({
  product: { type: Object, required: true },
  reviews: { type: Array, required: true }
})
const emit = defineEmits(['review-added'])
const isLoggedIn = ref(AuthService.checkAuth())
const isSubmitting = ref(false)

watch(() => localStorage.getItem('user'), () => {
  isLoggedIn.value = AuthService.checkAuth()
})

const newReview = ref({
  rating: 0,
  comment: ''
})

function formatDate(dateString) {
  const date = new Date(dateString)
  const year = date.getFullYear()
  const month = String(date.getMonth() + 1).padStart(2, '0')
  const day = String(date.getDate()).padStart(2, '0')
  const hours = String(date.getHours()).padStart(2, '0')
  const minutes = String(date.getMinutes()).padStart(2, '0')
  return `${day}/${month}/${year} ${hours}:${minutes}`
}

const submitReview = async () => {
  if (isSubmitting.value) return
    isSubmitting.value = true
  if (!newReview.value.rating) {
    alert('Vui lòng chọn số sao đánh giá!')
    return
  }
  if (!newReview.value.comment.trim()) {
    alert('Vui lòng nhập nhận xét!')
    return
  }

  const user = AuthService.getCurrentUser()
  if (!user || !user.maNguoiDung) {
    alert('Không thể lấy thông tin người dùng. Vui lòng đăng nhập lại.')
    return
  }

  const reviewPayload = {
    maSanPham: props.product.maSanPham,
    maNguoiDung: user.maNguoiDung,
    diemDanhGia: newReview.value.rating,
    noiDung: newReview.value.comment.trim()
  }

  try {
    const newReviewFromServer = await ProductService.submitReview(reviewPayload)
    const formattedReview = {
      id: newReviewFromServer.id,
      hoTen: user.hoTen || "Người dùng",
      soSao: newReviewFromServer.diemDanhGia,
      noiDung: newReviewFromServer.noiDung,
      ngayTao: newReviewFromServer.ngayTao
    }
    emit('review-added', formattedReview)
    newReview.value.rating = 0
    newReview.value.comment = ''
    alert('Gửi đánh giá thành công!')
  } catch (error) {
    alert(error.message || 'Gửi đánh giá thất bại. Vui lòng thử lại!')
  } finally {
    isSubmitting.value = false
  }
}
</script>

<style scoped>
.review-form {
  padding: 0.5rem;
  border-radius: 15px;
  margin-bottom: 2rem;
}

.review-form .form-label {
  font-weight: 500;
  font-size: 16px;
  color: var(--dark-color);
}

.review-form h5 {
  margin-bottom: 0.5rem;
  font-weight: 600;
}

.rating-input {
  display: flex;
  justify-content: start;
  flex-direction: row-reverse;
  gap: 1rem;
}

.rating-input input {
  display: none;
}

.rating-input label {
  cursor: pointer;
  font-size: 2rem;
  color: #ddd;
  transition: all 0.2s ease;
}

.rating-input label:hover,
.rating-input label:hover ~ label,
.rating-input input:checked ~ label {
  color: #ffc107;
}


textarea {
  width: 100%;
  padding: 0.5rem;
  margin-bottom: 0.5rem;
  border-radius: 8px;
  border: 1px solid #ccc;
  resize: vertical;
}

.btn-submit {
  padding: 0.5rem 1rem;
  background: linear-gradient(to right, #3b82f6, #10b981);
  color: white;
  border: none;
  border-radius: 50px;
  cursor: pointer;
}

.btn-submit:hover {
  opacity: 0.9;
}

.review-list {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.review-item {
  background-color: #fff;
  border: 1px solid #e0e0e0;
  border-radius: 12px;
  padding: 1rem 1.2rem;
  box-shadow: 0 2px 6px rgba(0,0,0,0.03);
}

.review-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.5rem;
}

.review-author-date {
  display: flex;
  flex-direction: row;
  gap: 0.5rem;
  align-items: center;
}

.review-author {
  font-weight: 600;
}

.review-date {
  font-size: 0.85rem;
  color: #888;
}

.review-rating i {
  color: #ffc107;
  margin-left: 2px;
}

.review-content {
  font-size: 0.95rem;
  color: #333;
  line-height: 1.4;
}

</style>