<template>
  <div class="auth-wrapper">
    <div class="auth-container">
      <div class="auth-tabs">
        <button class="auth-tab active" data-tab="login" @click="goToLogin">
          <i class="bi bi-person-check-fill"></i> Đăng nhập
        </button>
        <button class="auth-tab" @click="goToRegister">
          <i class="bi bi-person-plus-fill"></i> Đăng ký
        </button>
      </div>
      <div class="auth-form-container">
        <form @submit.prevent="handleSubmit" class="auth-form">
          <h2 class="auth-title">Đăng nhập Sport Elite</h2>
          <p class="auth-subtitle">Chào mừng bạn trở lại! Vui lòng đăng nhập để tiếp tục.</p>
          <div class="form-group">
            <label for="email">Email</label>
            <div class="input-group" :class="{ 'error': v$.email.$error }">
              <i class="bi bi-envelope"></i>
              <input 
                type="email" 
                id="email" 
                v-model="form.email"
                placeholder="Nhập email của bạn"
                @input="v$.email.$touch()"
              >
            </div>
            <div class="error-message" v-if="v$.email.$error">
              {{ v$.email.$errors[0].$message }}
            </div>
          </div>
          <div class="form-group">
            <label for="password">Mật khẩu</label>
            <div class="input-group" :class="{ 'error': v$.password.$error }">
              <i class="bi bi-lock"></i>
              <input 
                :type="showPassword ? 'text' : 'password'"
                id="password" 
                v-model="form.password"
                placeholder="Nhập mật khẩu"
                @input="v$.password.$touch()"
              >
              <i 
                class="bi toggle-password" 
                :class="showPassword ? 'bi-eye' : 'bi-eye-slash'"
                @click="showPassword = !showPassword"
              ></i>
            </div>
            <div class="error-message" v-if="v$.password.$error">
              {{ v$.password.$errors[0].$message }}
            </div>
          </div>
          <div class="form-options">
            <label class="remember-me">
              <input type="checkbox" class="checkbox" v-model="form.remember">
              <span>Ghi nhớ đăng nhập</span>
            </label>
            <router-link to="/forgot-password" class="forgot-password">
              Quên mật khẩu?
            </router-link>
          </div>
          <button type="submit" class="auth-button" :disabled="loading">
            <i class="bi bi-box-arrow-in-right"></i>
            {{ loading ? 'Đang đăng nhập...' : 'Đăng nhập' }}
          </button>
          <div class="social-divider">
            <span>Hoặc đăng nhập với</span>
          </div>
          <div class="social-buttons">
            <button type="button" class="social-button google" @click="loginWithGoogle">
              <img src="@/assets/images/auth/google.png" width="24" height="24" alt="Google">
              Google
            </button>
            <button type="button" class="social-button facebook" @click="loginWithFacebook">
              <img src="@/assets/images/auth/facebook.png" width="24" height="24" alt="Facebook">
              Facebook
            </button>
          </div>
          <div class="auth-footer">
            <p>Chưa có tài khoản? <router-link to="/dang-ky">Đăng ký ngay</router-link></p>
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
import { required, email, minLength } from '@vuelidate/validators'
import AuthService from '@/services/AuthService'

const router = useRouter()
const loading = ref(false)
const error = ref('')
const showPassword = ref(false)

const form = reactive({
  email: '',
  password: '',
  remember: false
})

const rules = {
  email: { required, email },
  password: { required, minLength: minLength(6) }
}

const v$ = useVuelidate(rules, form)

async function handleSubmit() {
  const isFormCorrect = await v$.value.$validate()
  if (!isFormCorrect) return
  try {
    loading.value = true
    await AuthService.login(form)
    await Swal.fire({
      icon: 'success',
      title: 'Đăng nhập thành công',
      text: 'Chào mừng bạn quay trở lại Sport Elite!',
      timer: 1500,
      showConfirmButton: false
    })
    router.push('/')
  } catch (err) {
    Swal.fire({
      icon: 'error',
      title: 'Đăng nhập thất bại',
      text: err?.message || 'Email hoặc mật khẩu không đúng'
    })
  } finally {
    loading.value = false
  }
}

function goToRegister() {
  router.push('/dang-ky')
}

function goToLogin() {
  router.push('/dang-nhap')
}

function loginWithGoogle() {
  console.log('Google login clicked')
}

function loginWithFacebook() {
  console.log('Facebook login clicked')
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
</style>