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
          <div class="mb-3"><i class="bi bi-chat-square-text" style="font-size: 2rem;"></i></div>
          <p class="text-muted">Chưa có đánh giá nào cho sản phẩm này</p>
          <p>Hãy là người đầu tiên đánh giá sản phẩm!</p>
        </div>
        <div v-else class="review-list-container">
          <div class="review-list">
            <div v-for="review in reviews" :key="review.id" class="review-item" :class="{ 'review-hidden': isHidden(review.id) }">
              <div class="review-header">
                <div class="review-author-date">
                  <span class="review-author">{{ review.hoTen || "Người dùng" }}</span>
                  <span class="review-date">{{ formatDate(review.ngayTao) }}</span>
                </div>
                <div class="review-rating">
                  <i v-for="star in 5" :key="star" class="bi"
                    :class="star <= review.diemDanhGia ? 'bi-star-fill' : 'bi-star'"></i>
                </div>
              </div>
              <div class="review-body mt-3">
                <div v-if="isHidden(review.id)" class="text-center text-muted py-3">
                  <p>Bình luận này đã bị ẩn</p>
                  <button class="btn btn-sm btn-outline-secondary" @click="unhideReview(review.id)">
                    <i class="bi bi-eye"></i> Hiện bình luận
                  </button>
                </div>
                <div v-else>
                  <div class="review-body-wrapper" v-if="!review.isEditing">
                    <div class="review-content flex-grow-1">
                      {{ review.noiDung }}
                    </div>
                    <div class="review-actions ms-3">
                      <div v-if="isOwner(review)" class="dropdown d-inline">
                        <button class="btn btn-sm btn-link text-muted p-0" data-bs-toggle="dropdown">
                          <i class="bi bi-three-dots-vertical fs-5"></i>
                        </button>
                        <ul class="dropdown-menu">
                          <li><button class="dropdown-item" @click="startEdit(review)"><i class="bi bi-pencil me-2"></i>Sửa</button></li>
                          <li><button class="dropdown-item text-danger" @click="deleteReview(review)"><i class="bi bi-trash me-2"></i>Xóa</button></li>
                        </ul>
                      </div>
                      <div v-else class="dropdown d-inline">
                        <button class="btn btn-sm btn-link text-muted p-0" data-bs-toggle="dropdown">
                          <i class="bi bi-three-dots-vertical fs-5"></i>
                        </button>
                        <ul class="dropdown-menu">
                          <li><button class="dropdown-item" @click="hideReview(review.id)"><i class="bi bi-eye-slash me-2"></i>Ẩn bình luận</button></li>
                          <li><button class="dropdown-item text-danger" @click="openReportModal(review)"><i class="bi bi-flag me-2"></i>Báo cáo bình luận</button></li>
                        </ul>
                      </div>
                    </div>
                  </div>
                  <div v-else class="edit-form">
                    <div class="mb-3">
                      <label class="form-label small fw-bold">Chỉnh sửa đánh giá</label>
                      <div class="rating-input d-flex gap-2 mb-3">
                        <input type="radio" :id="'edit-star5-' + review.id" value="5" v-model="review.editingRating">
                        <label :for="'edit-star5-' + review.id" title="Rất tốt">★</label>
                        <input type="radio" :id="'edit-star4-' + review.id" value="4" v-model="review.editingRating">
                        <label :for="'edit-star4-' + review.id" title="Tốt">★</label>
                        <input type="radio" :id="'edit-star3-' + review.id" value="3" v-model="review.editingRating">
                        <label :for="'edit-star3-' + review.id" title="Bình thường">★</label>
                        <input type="radio" :id="'edit-star2-' + review.id" value="2" v-model="review.editingRating">
                        <label :for="'edit-star2-' + review.id" title="Kém">★</label>
                        <input type="radio" :id="'edit-star1-' + review.id" value="1" v-model="review.editingRating">
                        <label :for="'edit-star1-' + review.id" title="Rất kém">★</label>
                      </div>
                      <textarea class="form-control" v-model="review.editingComment" rows="3"></textarea>
                    </div>
                    <div class="d-flex gap-2">
                      <button class="btn btn-sm btn-primary" @click="saveEdit(review)" :disabled="isSubmitting">Lưu thay đổi</button>
                      <button class="btn btn-sm btn-secondary" @click="cancelEdit(review)">Hủy</button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <div class="modal fade" id="reportModal" tabindex="-1">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Báo cáo bình luận</h5>
          <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
        </div>
        <div class="modal-body">
          <p><strong>Tại sao bạn báo cáo bình luận này?</strong></p>
          <p class="text-muted small">Nếu bạn nhận thấy ai đó đang gặp nguy hiểm, đừng chần chừ mà hãy tìm ngay sự giúp đỡ trước khi báo cáo.</p>
          <div class="form-check" v-for="reason in reportReasons" :key="reason">
            <input class="form-check-input" type="radio" name="reportReason" :id="'reason-'+reason" v-model="selectedReportReason" :value="reason">
            <label class="form-check-label" :for="'reason-'+reason">{{ reason }}</label>
          </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Hủy</button>
          <button type="button" class="btn btn-danger" @click="submitReport" :disabled="!selectedReportReason">Gửi báo cáo</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, watch, onMounted, onUnmounted } from 'vue'
import Swal from 'sweetalert2'
import AuthService from '@/services/AuthService'
import ProductService from '@/services/ProductService'
import { Modal } from 'bootstrap'

const props = defineProps({
  product: { type: Object, required: true },
  reviews: { type: Array, required: true }
})
const emit = defineEmits(['review-added', 'review-updated', 'review-deleted'])

const isLoggedIn = ref(false)
const currentUser = ref(null)
const tokenCheckInterval = ref(null)
const isSubmitting = ref(false)
const hiddenReviewIds = ref([])
const selectedReportReason = ref('')
const reportingReview = ref(null)

const reportReasons = [
  'Thông tin sai lệch về sản phẩm',
  'Lời lẽ thô tục, xúc phạm',
  'Quảng cáo, spam',
  'Nội dung không liên quan',
  'Hình ảnh không phù hợp',
  'Khác'
]

function updateAuthStatus() {
  isLoggedIn.value = AuthService.checkAuth()
  currentUser.value = AuthService.getCurrentUser()
}

onMounted(() => {
  updateAuthStatus()
  tokenCheckInterval.value = setInterval(updateAuthStatus, 60000)
})

onUnmounted(() => {
  if (tokenCheckInterval.value) {
    clearInterval(tokenCheckInterval.value)
  }
})

watch(() => localStorage.getItem('user'), () => {
  updateAuthStatus()
})

const newReview = ref({
  rating: 0,
  comment: ''
})

function formatDate(dateString) {
  if (!dateString) return ''
  const date = new Date(dateString)
  if (isNaN(date.getTime())) return 'Vừa xong'
  const year = date.getFullYear()
  const month = String(date.getMonth() + 1).padStart(2, '0')
  const day = String(date.getDate()).padStart(2, '0')
  const hours = String(date.getHours()).padStart(2, '0')
  const minutes = String(date.getMinutes()).padStart(2, '0')
  return `${day}/${month}/${year} ${hours}:${minutes}`
}

function isOwner(review) {
  return currentUser.value && review.maNguoiDung === currentUser.value.maNguoiDung
}

function isHidden(reviewId) {
  return hiddenReviewIds.value.includes(reviewId)
}

function hideReview(reviewId) {
  if (!hiddenReviewIds.value.includes(reviewId)) {
    hiddenReviewIds.value.push(reviewId)
  }
}

function unhideReview(reviewId) {
  hiddenReviewIds.value = hiddenReviewIds.value.filter(id => id !== reviewId)
}

function openReportModal(review) {
  reportingReview.value = review
  selectedReportReason.value = ''
  const modal = new Modal(document.getElementById('reportModal'))
  modal.show()
}

function submitReport() {
  Swal.fire({
    icon: 'success',
    title: 'Đã gửi báo cáo',
    html: `
      <div style="line-height:1.6">
        <strong>Lý do:</strong> "${selectedReportReason.value}"<br/>
        <span style="color:#6b7280; font-size:15px;">
          Cảm ơn bạn đã góp phần giữ cộng đồng sạch đẹp!
        </span>
      </div>
    `,
    confirmButtonText: 'Ok',
    confirmButtonColor: '#2563eb'
  })
  const modalEl = document.getElementById('reportModal')
  const modal = Modal.getInstance(modalEl)
  modal.hide()
}

function startEdit(review) {
  review.isEditing = true
  review.editingRating = review.diemDanhGia
  review.editingComment = review.noiDung
}

function cancelEdit(review) {
  review.isEditing = false
  delete review.editingRating
  delete review.editingComment
}

const saveEdit = async (review) => {
  if (!review.editingRating || !review.editingComment.trim()) {
    await Swal.fire({
      icon: 'warning',
      title: 'Thiếu thông tin',
      text: 'Vui lòng nhập đầy đủ đánh giá và nội dung!'
    })
    return
  }
  isSubmitting.value = true
  try {
    await ProductService.updateReview(review.id, {
      diemDanhGia: review.editingRating,
      noiDung: review.editingComment.trim()
    })
    review.diemDanhGia = review.editingRating
    review.noiDung = review.editingComment.trim()
    review.isEditing = false
    await Swal.fire({
      icon: 'success',
      title: 'Cập nhật đánh giá thành công',
      text: 'Đánh giá của bạn đã được cập nhật thành công!'
    })
    emit('review-updated')
  } catch (error) {
    await Swal.fire({
      icon: 'error',
      title: 'Cập nhật thất bại',
      text: error.message || 'Vui lòng thử lại.'
    })
  } finally {
    isSubmitting.value = false
  }
}

const deleteReview = async (review) => {
  const result = await Swal.fire({
    icon: 'warning',
    title: 'Xóa đánh giá?',
    text: 'Bạn có chắc chắn muốn xóa đánh giá này?',
    showCancelButton: true,
    confirmButtonText: 'Xóa',
    cancelButtonText: 'Hủy',
    confirmButtonColor: '#ef4444'
  })
  if (!result.isConfirmed) return
  isSubmitting.value = true
  try {
    await ProductService.deleteReview(review.id)
    emit('review-deleted')
    await Swal.fire({
      icon: 'success',
      title: 'Xóa đánh giá thành công',
      text: 'Đánh giá của bạn đã bị xóa thành công!'
    })
  } catch (error) {
    await Swal.fire({
      icon: 'error',
      title: 'Xóa thất bại',
      text: error.message || 'Vui lòng thử lại.'
    })
  } finally {
    isSubmitting.value = false
  }
}

const submitReview = async () => {
  if (isSubmitting.value) return
  isSubmitting.value = true
  if (!newReview.value.rating) {
    await Swal.fire({
      icon: 'warning',
      title: 'Thiếu đánh giá',
      text: 'Vui lòng chọn số sao đánh giá!'
    })
    isSubmitting.value = false
    return
  }
  if (!newReview.value.comment.trim()) {
    await Swal.fire({
      icon: 'warning',
      title: 'Thiếu nhận xét',
      text: 'Vui lòng nhập nhận xét của bạn!'
    })
    isSubmitting.value = false
    return
  }
  const user = currentUser.value
  if (!user || !user.maNguoiDung) {
    await Swal.fire({
      icon: 'error',
      title: 'Lỗi đăng nhập',
      text: 'Không thể lấy thông tin người dùng. Vui lòng đăng nhập lại.'
    })
    isSubmitting.value = false
    return
  }
  try {
    const reviewPayload = {
      maSanPham: props.product.maSanPham,
      maNguoiDung: user.maNguoiDung,
      diemDanhGia: newReview.value.rating,
      noiDung: newReview.value.comment.trim()
    }
    const newReviewFromServer = await ProductService.submitReview(reviewPayload)
    emit('review-added', {
      id: newReviewFromServer.id,
      hoTen: user.hoTen || 'Người dùng',
      maNguoiDung: user.maNguoiDung,
      diemDanhGia: newReviewFromServer.diemDanhGia,
      noiDung: newReviewFromServer.noiDung,
      ngayTao: newReviewFromServer.ngayTao
    })
    newReview.value.rating = 0
    newReview.value.comment = ''
    await Swal.fire({
      icon: 'success',
      title: 'Gửi đánh giá thành công 🎉',
      text: 'Cảm ơn bạn đã chia sẻ trải nghiệm!'
    })
  } catch (error) {
    await Swal.fire({
      icon: 'error',
      title: 'Gửi đánh giá thất bại',
      text: error.message || 'Vui lòng thử lại sau.'
    })
  } finally {
    isSubmitting.value = false
  }
}
</script>

<style scoped>
.review-list-container {
  max-height: 600px;
  overflow-y: auto;
  padding-right: 8px;
  margin-top: 1rem;
}

.review-list-container::-webkit-scrollbar {
  width: 8px;
}

.review-list-container::-webkit-scrollbar-track {
  background: #f1f1f1;
  border-radius: 10px;
}

.review-list-container::-webkit-scrollbar-thumb {
  background: #c0c0c0;
  border-radius: 10px;
}

.review-list-container::-webkit-scrollbar-thumb:hover {
  background: #a8a8a8;
}

.review-list-container {
  scrollbar-width: thin;
  scrollbar-color: #c0c0c0 #f1f1f1;
}

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
  white-space: pre-wrap;
  line-height: 1.4;
}

.edit-form textarea {
  font-size: 0.95rem;
}

.review-hidden {
  position: relative;
  background-color: #f8f9fa;
  border-radius: 12px;
}

.review-hidden::before {
  content: '';
  position: absolute;
  top: 0; 
  left: 0; 
  right: 0; 
  bottom: 0;
  background: rgba(255, 255, 255, 0.85);
  backdrop-filter: blur(2px);
  border-radius: 12px;
  z-index: 1;
  pointer-events: none;
}

.review-hidden > .review-body > .text-center {
  position: relative;
  z-index: 2;
  border-radius: 8px;
}

.review-body-wrapper {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
}

.review-content {
  flex-grow: 1;
  font-size: 0.95rem;
  color: #333;
  white-space: pre-wrap;
  line-height: 1.4;
}

.review-actions {
  flex-shrink: 0;
}

.dropdown-menu {
  font-size: 0.9rem;
}

.dropdown-menu .dropdown-item {
  margin-top: 5px;
}

@media (max-width: 1200px) {
  .review-list-container {
    max-height: 550px;
  }
  
  .rating-input label {
    font-size: 1.9rem;
  }
  
  .review-form .form-label {
    font-size: 15px;
  }
}

@media (max-width: 992px) {
  .review-list-container {
    max-height: 500px;
    padding-right: 6px;
  }
  
  .review-form {
    margin-bottom: 1.5rem;
  }
  
  .rating-input {
    gap: 0.9rem;
  }
  
  .rating-input label {
    font-size: 1.8rem;
  }
  
  .review-item {
    padding: 0.9rem 1rem;
  }
  
  .review-author {
    font-size: 0.95rem;
  }
  
  .review-date {
    font-size: 0.8rem;
  }
  
  .review-content {
    font-size: 0.9rem;
  }
}

@media (max-width: 768px) {
  .review-list-container {
    max-height: 450px;
    padding-right: 5px;
  }
  
  .review-list-container::-webkit-scrollbar {
    width: 6px;
  }
  
  .review-form {
    padding: 0.4rem;
    margin-bottom: 1.2rem;
    border-radius: 12px;
  }
  
  .review-form .form-label {
    font-size: 14px;
  }
  
  .rating-input {
    gap: 0.8rem;
  }
  
  .rating-input label {
    font-size: 1.7rem;
  }
  
  textarea {
    padding: 0.45rem;
    font-size: 0.9rem;
  }
  
  .btn-submit {
    padding: 0.45rem 0.9rem;
    font-size: 0.95rem;
  }
  
  .review-list {
    gap: 0.8rem;
  }
  
  .review-item {
    padding: 0.8rem 0.9rem;
    border-radius: 10px;
  }
  
  .review-header {
    margin-bottom: 0.4rem;
  }
  
  .review-author-date {
    gap: 0.4rem;
    flex-wrap: wrap;
  }
  
  .review-author {
    font-size: 0.9rem;
  }
  
  .review-date {
    font-size: 0.75rem;
  }
  
  .review-rating i {
    font-size: 0.9rem;
  }
  
  .review-content {
    font-size: 0.88rem;
    line-height: 1.5;
  }
  
  .review-body-wrapper {
    flex-direction: column;
    gap: 0.5rem;
  }
  
  .review-actions {
    align-self: flex-end;
  }
  
  .dropdown-menu {
    font-size: 0.85rem;
  }
  
  .edit-form textarea {
    font-size: 0.88rem;
  }
}

@media (max-width: 576px) {
  .review-list-container {
    max-height: 400px;
    padding-right: 4px;
  }
  
  .review-form {
    padding: 0.3rem;
    margin-bottom: 1rem;
    border-radius: 10px;
  }
  
  .review-form .form-label {
    font-size: 13px;
  }
  
  .rating-input {
    gap: 0.7rem;
  }
  
  .rating-input label {
    font-size: 1.6rem;
  }
  
  textarea {
    padding: 0.4rem;
    font-size: 0.85rem;
    border-radius: 6px;
  }
  
  .btn-submit {
    padding: 0.4rem 0.8rem;
    font-size: 0.9rem;
    border-radius: 40px;
  }
  
  .review-list {
    gap: 0.7rem;
  }
  
  .review-item {
    padding: 0.7rem 0.8rem;
    border-radius: 8px;
  }
  
  .review-author {
    font-size: 0.88rem;
  }
  
  .review-date {
    font-size: 0.72rem;
  }
  
  .review-rating i {
    font-size: 0.85rem;
    margin-left: 1px;
  }
  
  .review-content {
    font-size: 0.85rem;
    line-height: 1.5;
  }
  
  .dropdown-menu {
    font-size: 0.8rem;
  }
  
  .dropdown-menu .dropdown-item {
    padding: 0.4rem 0.8rem;
  }
  
  .edit-form {
    font-size: 0.85rem;
  }
  
  .edit-form .form-label {
    font-size: 12px;
  }
  
  .edit-form .rating-input {
    gap: 0.6rem;
  }
  
  .edit-form .rating-input label {
    font-size: 1.5rem;
  }
  
  .edit-form textarea {
    font-size: 0.85rem;
  }
  
  .edit-form .btn {
    font-size: 0.85rem;
    padding: 0.35rem 0.7rem;
  }
}

@media (max-width: 400px) {
  .review-list-container {
    max-height: 350px;
  }
  
  .rating-input {
    gap: 0.5rem;
  }
  
  .rating-input label {
    font-size: 1.5rem;
  }
  
  .review-form .form-label {
    font-size: 12px;
  }
  
  textarea {
    padding: 0.35rem;
    font-size: 0.8rem;
  }
  
  .btn-submit {
    padding: 0.35rem 0.7rem;
    font-size: 0.85rem;
  }
  
  .review-item {
    padding: 0.6rem 0.7rem;
  }
  
  .review-author {
    font-size: 0.85rem;
  }
  
  .review-date {
    font-size: 0.7rem;
  }
  
  .review-rating i {
    font-size: 0.8rem;
  }
  
  .review-content {
    font-size: 0.8rem;
  }
}

.alert {
  font-size: 0.95rem;
}

@media (max-width: 768px) {
  .alert {
    font-size: 0.9rem;
    padding: 0.8rem;
  }
}

@media (max-width: 576px) {
  .alert {
    font-size: 0.85rem;
    padding: 0.7rem;
  }
}

@media (max-width: 576px) {
  .modal-dialog {
    margin: 0.5rem;
  }
  
  .modal-title {
    font-size: 1.1rem;
  }
  
  .modal-body {
    font-size: 0.9rem;
  }
  
  .modal-body p {
    font-size: 0.85rem;
  }
  
  .form-check-label {
    font-size: 0.85rem;
  }
  
  .modal-footer .btn {
    font-size: 0.85rem;
    padding: 0.4rem 0.8rem;
  }
}
</style>