<template>
  <div class="auth-wrapper">
    <div class="auth-container">
      <div class="auth-tabs">
        <button class="auth-tab" @click="goToLogin">
          <i class="bi bi-person-check-fill"></i> Đăng nhập
        </button>
        <button class="auth-tab active" @click="goToRegister">
          <i class="bi bi-person-plus-fill"></i> Đăng ký
        </button>
      </div>
      <div class="auth-form-container">
        <form @submit.prevent="handleSubmit" class="auth-form">
          <h2 class="auth-title">Đăng ký tài khoản</h2>
          <p class="auth-subtitle">Tham gia cùng Sport Elite ngay hôm nay!</p>
          <div class="form-group">
            <label for="username">Tên đăng nhập</label>
            <div class="input-group" :class="{ 'error': v$.tenDangNhap.$error }">
              <i class="bi bi-person"></i>
              <input 
                type="text" 
                id="username"
                v-model="form.tenDangNhap"
                placeholder="Nhập tên đăng nhập"
                @input="v$.tenDangNhap.$touch()"
              >
            </div>
            <div class="error-message" v-if="v$.tenDangNhap.$error">
              {{ v$.tenDangNhap.$errors[0].$message }}
            </div>
          </div>
          <div class="form-group">
            <label for="hoTen">Họ và tên</label>
            <div class="input-group" :class="{ 'error': v$.hoTen.$error }">
              <i class="bi bi-person-badge"></i>
              <input 
                type="text" 
                id="hoTen"
                v-model="form.hoTen"
                placeholder="Nhập họ và tên"
                @input="v$.hoTen.$touch()"
              >
            </div>
            <div class="error-message" v-if="v$.hoTen.$error">
              {{ v$.hoTen.$errors[0].$message }}
            </div>
          </div>
          <div class="form-group">
            <label for="email">Email</label>
            <div class="input-group" :class="{ 'error': v$.email.$error }">
              <i class="bi bi-envelope"></i>
              <input 
                type="email" 
                id="email"
                v-model="form.email"
                placeholder="Nhập email"
                @input="v$.email.$touch()"
              >
            </div>
            <div class="error-message" v-if="v$.email.$error">
              {{ v$.email.$errors[0].$message }}
            </div>
          </div>
          <div class="form-group">
            <label for="soDienThoai">Số điện thoại</label>
            <div class="input-group">
              <i class="bi bi-phone"></i>
              <input 
                type="tel" 
                id="soDienThoai"
                v-model="form.soDienThoai"
                placeholder="Nhập số điện thoại"
              >
            </div>
          </div>
          <div class="form-group">
            <label for="diaChi">Địa chỉ</label>
            <div class="input-group">
              <i class="bi bi-geo-alt"></i>
              <input 
                type="text" 
                id="diaChi"
                v-model="form.diaChi"
                placeholder="Nhập địa chỉ"
              >
            </div>
          </div>
          <div class="form-group">
            <label for="password">Mật khẩu</label>
            <div class="input-group" :class="{ 'error': v$.matKhau.$error }">
              <i class="bi bi-lock"></i>
              <input 
                :type="showPassword ? 'text' : 'password'"
                id="password"
                v-model="form.matKhau"
                placeholder="Nhập mật khẩu"
                @input="v$.matKhau.$touch()"
              >
              <i 
                class="bi toggle-password" 
                :class="showPassword ? 'bi-eye' : 'bi-eye-slash'"
                @click="showPassword = !showPassword"
              ></i>
            </div>
            <div class="error-message" v-if="v$.matKhau.$error">
              {{ v$.matKhau.$errors[0].$message }}
            </div>
          </div>
          <div class="form-group">
            <label for="confirmPassword">Nhập lại mật khẩu</label>
            <div class="input-group" :class="{ 'error': v$.confirmPassword.$error }">
              <i class="bi bi-lock"></i>
              <input 
                :type="showConfirmPassword ? 'text' : 'password'"
                id="confirmPassword"
                v-model="form.confirmPassword"
                placeholder="Nhập lại mật khẩu"
                @input="v$.confirmPassword.$touch()"
              >
              <i 
                class="bi toggle-password" 
                :class="showConfirmPassword ? 'bi-eye' : 'bi-eye-slash'"
                @click="showConfirmPassword = !showConfirmPassword"
              ></i>
            </div>
            <div class="error-message" v-if="v$.confirmPassword.$error">
              {{ v$.confirmPassword.$errors[0].$message }}
            </div>
          </div>
          <button 
            type="submit" 
            class="auth-button" 
            :disabled="loading || v$.$invalid"
            :class="{ 'loading': loading }"
            >
            <i class="bi" :class="loading ? 'bi-arrow-repeat spin' : 'bi-person-plus'"></i>
            {{ loading ? 'Đang đăng ký...' : 'Đăng ký' }}
          </button>
          <div class="auth-footer">
            <p>Đã có tài khoản? <router-link to="/dang-nhap">Đăng nhập ngay</router-link></p>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import Swal from 'sweetalert2'
import { useVuelidate } from '@vuelidate/core'
import { required, email, minLength, sameAs } from '@vuelidate/validators'
import AuthService from '@/services/AuthService'

const router = useRouter()
const loading = ref(false)
const error = ref('')
const showPassword = ref(false)
const showConfirmPassword = ref(false)

const form = reactive({
  tenDangNhap: '',
  hoTen: '',
  email: '',
  soDienThoai: '',
  diaChi: '',
  matKhau: '',
  confirmPassword: ''
})

const rules = {
  tenDangNhap: { required, minLength: minLength(3) },
  hoTen: { required },
  email: { required, email },
  matKhau: { required, minLength: minLength(6) },
  confirmPassword: { 
    required,
    sameAsPassword: { 
      $validator: value => value === form.matKhau,
      $message: 'Mật khẩu nhập lại không khớp'
    }
  }
}

const v$ = useVuelidate(rules, form)

async function handleSubmit() {
  const isFormCorrect = await v$.value.$validate()
  if (!isFormCorrect) {
    Swal.fire({
      icon: 'warning',
      title: 'Thông tin chưa hợp lệ',
      text: 'Vui lòng kiểm tra lại các trường đã nhập'
    })
    return
  }
  try {
    loading.value = true
    await AuthService.register({
      tenDangNhap: form.tenDangNhap,
      hoTen: form.hoTen,
      email: form.email,
      soDienThoai: form.soDienThoai || null,
      diaChi: form.diaChi || null,
      matKhau: form.matKhau
    })
    await Swal.fire({
      icon: 'success',
      title: 'Đăng ký thành công',
      text: 'Bạn có thể đăng nhập ngay bây giờ',
      confirmButtonText: 'Đăng nhập'
    })
    router.push('/dang-nhap')
  } catch (err) {
    Swal.fire({
      icon: 'error',
      title: 'Đăng ký thất bại',
      text: err?.message || 'Vui lòng thử lại sau'
    })
  } finally {
    loading.value = false
  }
}

function goToLogin() {
  router.push('/dang-nhap')
}

function goToRegister() {
  router.push('/dang-ky')
}
</script>

<style scoped>
.auth-wrapper {
  display: flex;
  align-items: center;
  background: linear-gradient(45deg, rgba(54,124,215,0.03), rgba(52,168,83,0.03));
  padding: 40px 0;
}

.auth-container {
  max-width: 480px;
  width: 100%;
  margin: 0 auto;
  padding: 0 20px;
}

.auth-tabs {
  display: flex;
  background: white;
  padding: 6px;
  border-radius: 16px;
  margin-bottom: 25px;
  box-shadow: var(--shadow-sm);
}

.auth-tab {
  flex: 1;
  padding: 14px;
  border: none;
  background: none;
  border-radius: 12px;
  color: #555;
  font-weight: 500;
  transition: all 0.3s ease;
  font-size: 15px;
}

.auth-tab i {
  margin-right: 8px;
  font-size: 18px;
}

.auth-tab.active {
  background: var(--gradient);
  color: white;
  box-shadow: var(--shadow-md);
}

.auth-form-container {
  background: white;
  border-radius: 24px;
  padding: 40px;
  box-shadow: var(--shadow-md);
}

.auth-title {
  font-size: 28px;
  font-weight: 700;
  text-align: center;
  margin-bottom: 15px;
  background: var(--gradient);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  letter-spacing: -0.5px;
}

.auth-subtitle {
  color: #666;
  margin-bottom: 30px;
  font-size: 15px;
}

.form-group {
  margin-bottom: 24px;
}

.form-options {
  margin-bottom: 24px;
}

.form-options .remember-me .checkbox {
  margin-right: 5px;
}
.form-options .remember-me span {
  margin-right: 5px;
}
.form-group label {
  display: block;
  margin-bottom: 8px;
  font-weight: 500;
  color: #444;
  font-size: 14px;
}

.input-group {
  position: relative;
}

.input-group input {
  width: 100%;
  padding: 14px 48px 14px 48px;
  border: 2px solid #eef0f4;
  border-radius: 12px;
  font-size: 15px;
  transition: all 0.3s ease;
  color: #333;
}

.input-group i {
  position: absolute;
  top: 50%;
  transform: translateY(-50%);
  color: #aaa;
  font-size: 18px;
  transition: all 0.3s ease;
}

.input-group i:first-child {
  left: 16px;
}

.toggle-password {
  right: 16px;
  cursor: pointer;
}

.input-group input:focus {
  border-color: #367cd7;
  box-shadow: 0 0 0 4px rgba(54,124,215,0.1);
}

.input-group input:focus + i {
  color: #367cd7;
}

.auth-button {
  width: 100%;
  padding: 16px;
  border: none;
  border-radius: 12px;
  background: var(--gradient);
  color: white;
  font-size: 16px;
  font-weight: 600;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  cursor: pointer;
  transition: all 0.3s ease;
}

.auth-button:hover {
  background: var(--gradient-hover);
  transform: translateY(-2px);
  box-shadow: var(--shadow-lg);
}

.social-divider {
  margin: 30px 0;
  position: relative;
  text-align: center;
}

.social-divider::before,
.social-divider::after {
  content: '';
  position: absolute;
  top: 50%;
  width: calc(40% - 40px);
  height: 1px;
  background: #eef0f4;
}

.social-divider::before { left: 0; }
.social-divider::after { right: 0; }

.social-divider span {
  background: white;
  padding: 0 16px;
  color: #666;
  font-size: 14px;
}

.social-buttons {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 16px;
  margin-bottom: 24px;
}

.social-button {
  padding: 12px;
  border: 2px solid #eef0f4;
  border-radius: 12px;
  background: white;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  font-weight: 500;
  color: #444;
  transition: all 0.3s ease;
}

.social-button:hover {
  border-color: #367cd7;
  background: rgba(54,124,215,0.03);
  transform: translateY(-2px);
}

.auth-footer {
  text-align: center;
  margin-top: 24px;
  color: #666;
  font-size: 14px;
}

.auth-footer a {
  color: #367cd7;
  font-weight: 500;
  text-decoration: none;
}

.auth-footer a:hover {
  text-decoration: underline;
}

.auth-button.loading {
  opacity: 0.8;
  cursor: not-allowed;
}

.spin {
  animation: rotate 1s linear infinite;
}

@keyframes rotate {
  from { transform: rotate(0deg); }
  to { transform: rotate(360deg); }
}
</style>