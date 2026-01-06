<script setup>
import { ref, computed, onMounted, watch } from 'vue'
import { useRouter } from 'vue-router'
import AuthService from '@/services/AuthService'
import { useCartStore } from '@/stores/cart'
const router = useRouter()
const user = ref(null)
const isLoggedIn = computed(() => !!user.value)
const username = computed(() => user.value.hoTen || '')
const cartStore = useCartStore()

const cartCount = computed(() => {
  return Object.values(cartStore.items).reduce((sum, item) => sum + item.quantity, 0) 
})

onMounted(() => {
  const stored = localStorage.getItem('user')
  user.value = stored ? JSON.parse(stored) : null
})

watch(() => localStorage.getItem('user'), (newVal) => {
  user.value = newVal ? JSON.parse(newVal) : null
})

function logout() {
  AuthService.logout()
  localStorage.removeItem('user')
  user.value = null
  router.push('/dang-nhap')
}
</script>

<template>
  <nav class="navbar navbar-expand-lg navbar-light fixed-top">
    <div class="container">
      <router-link class="navbar-brand" to="/">
        <span class="gradient-text">Sport</span><strong>Elite</strong>
      </router-link>
      <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse" id="navbarNav">
        <ul class="navbar-nav ms-auto">
          <li class="nav-item">
            <router-link class="nav-link btn-hover px-4" to="/">
              <i class="bi bi-house-door-fill me-2"></i>Trang chủ
            </router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link btn-hover px-4" to="/gioi-thieu">
              <i class="bi bi-info-circle-fill me-2"></i>Giới thiệu
            </router-link>
          </li>
          <li class="nav-item dropdown">
            <router-link class="nav-link btn-hover px-4 dropdown-toggle" to="/san-pham">
              <i class="bi bi-grid-fill me-2"></i>Sản phẩm
            </router-link>
            <div class="submenu">
              <div class="submenu-inner">
                <div v-for="(category, index) in categories" :key="index" class="submenu-column">
                  <h6 class="submenu-title">{{ category.title }}</h6>
                  <ul class="submenu-list">
                    <li v-for="(item, idx) in category.items" :key="idx">
                      <router-link :to="item.link">{{ item.name }}</router-link>
                    </li>
                  </ul>
                </div>
              </div>
            </div>
          </li>
          <li class="nav-item">
          <router-link class="nav-link btn-hover px-4" to="/bai-viet">
              <i class="bi bi-newspaper me-2"></i>Bài viết
            </router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link btn-hover px-4" to="/lien-he">
              <i class="bi bi-envelope-fill me-2"></i>Liên hệ
            </router-link>
          </li>
          <li class="nav-item nav-item-action ms-lg-4">
            <router-link class="nav-link cart-btn" to="/gio-hang">
              <i class="bi bi-cart3"></i>
              <span class="cart-badge">{{ cartCount }}</span>
            </router-link>
          </li>
          <li class="nav-item nav-item-action ms-lg-3">
            <div v-if="isLoggedIn" class="dropdown">
              <a class="nav-link user-menu dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown">
                <i class="bi bi-person-circle me-2"></i>
                <span>{{ username }}</span>
              </a>
              <ul class="dropdown-menu dropdown-menu-end user-dropdown">
                <li>
                  <router-link class="dropdown-item" to="/tai-khoan">
                    <i class="bi bi-person me-2"></i>Tài khoản
                  </router-link>
                </li>
                <li>
                  <router-link class="dropdown-item" to="/don-hang">
                    <i class="bi bi-bag me-2"></i>Đơn hàng
                  </router-link>
                </li>
                <li><hr class="dropdown-divider"></li>
                <li>
                  <a class="dropdown-item text-danger" href="#" @click.prevent="logout">
                    <i class="bi bi-box-arrow-right me-2"></i>Đăng xuất
                  </a>
                </li>
              </ul>
            </div>
            <router-link v-else class="nav-link login-btn" to="/dang-nhap">
              <i class="bi bi-person me-2"></i>Đăng nhập
            </router-link>
          </li>
        </ul>
      </div>
    </div>
  </nav>
</template>

<style scoped>
.navbar {
  background: rgba(255, 255, 255, 0.95) !important;
  backdrop-filter: blur(10px);
  box-shadow: 0 2px 20px rgba(0,0,0,0.05);
  padding: 1.2rem 0;
  transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
}

.navbar.scrolled {
  padding: 12px 0;
  background: rgba(255, 255, 255, 0.98) !important;
  backdrop-filter: blur(10px);
  box-shadow: 0 5px 30px rgba(0,0,0,0.1);
  transform: translateY(-100%);
  animation: slideDown 0.5s forwards;
}

@keyframes slideDown {
  to {
    transform: translateY(0);
  }
}

.navbar.scrolled .nav-link {
  padding: 8px 16px;
}

.navbar.scrolled .navbar-brand {
  transform: scale(0.9);
}

.navbar-brand {
  font-size: 1.8rem;
  font-weight: 700;
  background: linear-gradient(120deg, #367cd7, #34a853);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  padding: 0;
  margin: 0;
  line-height: 1;
}

.navbar-brand strong {
  font-weight: 800;
  color: #003780;
}

.nav-link {
  font-weight: 500;
  font-size: 1rem;
  padding: 0.5rem 1rem;
  margin: 0 0.2rem;
  border-radius: 30px;
  transition: all 0.3s ease;
}

.nav-link:hover {
  color: #003780;
  transform: translateY(-2px);
}

.btn-hover {
  position: relative;
  overflow: hidden;
  z-index: 1;
}

.btn-hover::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  width: 0;
  height: 100%;
  background: linear-gradient(90deg, rgba(26,115,232,0.1), rgba(52,168,83,0.1));
  transition: width 0.3s ease;
  z-index: -1;
  border-radius: 30px;
}

.btn-hover:hover::before {
  width: 100%;
}

.btn-hover-light::before {
  background: rgba(255,255,255,0.2);
}

.navbar .btn-primary {
  padding: 0.4rem 1.2rem;
  box-shadow: 0 4px 15px rgba(26,115,232,0.15);
}

.navbar .btn-primary:hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 25px rgba(26,115,232,0.25);
}

.nav-item.dropdown {
  position: static;
}

.dropdown-menu {
  width: 100%;
  padding: 1.5rem;
  border: none;
  border-radius: 0 0 15px 15px;
  box-shadow: 0 10px 30px rgba(0,0,0,0.1);
  margin-top: 0;
  background: rgba(255, 255, 255, 0.98);
  backdrop-filter: blur(10px);
  display: none;
  opacity: 0;
  transform: translateY(20px);
  transition: all 0.3s ease;
}

.dropdown-menu.show {
  display: block;
  opacity: 1;
  transform: translateY(0);
}

.mega-menu {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 2rem;
}

.cart-btn {
  position: relative;
  padding: 0.625rem 1.125rem !important;
  background: #f8f9fa;
  border: 1.5px solid #003780;
  border-radius: 50px !important;
  transition: all 0.3s ease !important;
  color: #003780 !important;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  height: 42px;
  font-weight: 500;
}

.cart-btn i {
  font-size: 1.2rem;
}

.cart-btn:hover {
  background: #003780;
  color: white !important;
  transform: translateY(-2px);
}

.cart-badge {
  position: absolute;
  top: -6px;
  right: -6px;
  background: #003780;
  color: white;
  font-size: 0.75rem;
  min-width: 20px;
  height: 20px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.3s ease;
  border: 2px solid white;
  padding: 0 4px;
}

.cart-btn:hover .cart-badge {
  background: white;
  color: #003780;
  border-color: #003780;
}

.login-btn {
  padding: 0.625rem 1.5rem !important;
  background: linear-gradient(120deg, #367cd7, #34a853);
  border-radius: 50px !important;
  color: white !important;
  font-weight: 500;
  transition: all 0.3s ease !important;
  border: none;
  box-shadow: 0 4px 15px rgba(0,0,0,0.1);
  height: 42px;
  display: flex;
  align-items: center;
}

.user-menu {
  padding: 0.625rem 1.5rem !important;
  background: #f8f9fa;
  border-radius: 50px !important;
  color: #003780 !important;
  font-weight: 500;
  transition: all 0.3s ease !important;
  display: flex;
  align-items: center;
  height: 42px;
  border: 1.5px solid #003780;
}

.user-menu:hover,
.user-menu:focus {
  background: #003780;
  color: white !important;
  transform: translateY(-2px);
}

.user-menu i {
  font-size: 1.2rem;
}

.user-dropdown {
  min-width: 220px;
  padding: 0.5rem;
  border: none;
  border-radius: 15px;
  box-shadow: 0 10px 30px rgba(0,0,0,0.1);
  margin-top: 10px !important;
  display: none;
}

.dropdown-menu.show {
  display: block;
  animation: fadeInDown 0.3s ease;
}

.user-dropdown::before {
  content: '';
  position: absolute;
  top: -10px;
  right: 20px;
  border-left: 10px solid transparent;
  border-right: 10px solid transparent;
  border-bottom: 10px solid white;
}

.user-dropdown .dropdown-item {
  padding: 0.7rem 1rem;
  border-radius: 10px;
  display: flex;
  align-items: center;
  font-size: 0.95rem;
  transition: all 0.3s ease;
  color: #202124;
}

.user-dropdown .dropdown-item:hover {
  background: #f8f9fa;
  color: #003780;
  transform: translateX(5px);
}

.user-dropdown .dropdown-item i {
  font-size: 1.1rem;
  width: 24px;
}

.user-dropdown .dropdown-divider {
  margin: 0.5rem 0;
  opacity: 0.1;
}

.text-danger {
  color: #dc3545 !important;
}

.text-danger:hover {
  color: #dc3545 !important;
  background: rgba(220, 53, 69, 0.1) !important;
}

@keyframes fadeInDown {
  from {
    opacity: 0;
    transform: translateY(-10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@media (max-width: 991px) {
  .user-dropdown {
    width: 100%;
    margin: 0.5rem 0 !important;
    position: static !important;
    transform: none !important;
    box-shadow: none;
    background: #f8f9fa;
  }

  .user-dropdown::before {
    display: none;
  }

  .user-dropdown .dropdown-item {
    padding: 0.8rem 1rem;
  }
}

.submenu {
  position: absolute;
  top: 100%;
  left: 50%;
  transform: translateX(-50%);
  width: 80%;
  max-width: 1200px;
  background: rgba(255,255,255,0.98);
  backdrop-filter: blur(10px);
  border-radius: 15px;
  box-shadow: 0 10px 40px rgba(0,0,0,0.1);
  padding: 25px;
  opacity: 0;
  visibility: hidden;
  z-index: 1000;
  margin-top: 15px;
  transition: all 0.3s ease;
}

.nav-item.dropdown:hover .submenu {
  opacity: 1;
  visibility: visible;
  margin-top: 0;
}

.submenu::before {
  content: '';
  position: absolute;
  top: -10px;
  left: 50%;
  transform: translateX(-50%);
  border-left: 10px solid transparent;
  border-right: 10px solid transparent;
  border-bottom: 10px solid rgba(255,255,255,0.98);
}

.submenu-inner {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 30px;
}

.submenu-title {
  color: #003780;
  font-weight: 600;
  margin-bottom: 15px;
  font-size: 1rem;
}

.submenu-list {
  list-style: none;
  padding: 0;
  margin: 0;
}

.submenu-list li {
  margin-bottom: 10px;
}

.submenu-list a {
  color: #666;
  text-decoration: none;
  font-size: 0.95rem;
  transition: all 0.2s ease;
  display: flex;
  align-items: center;
}

.submenu-list a:hover {
  color: #003780;
  transform: translateX(5px);
}

@media (max-width: 991px) {
  .submenu {
    position: static;
    width: 100%;
    transform: none;
    margin-top: 0;
    padding: 10px;
    box-shadow: none;
    background: #f8f9fa;
  }

  .submenu::before {
    display: none;
  }

  .submenu-inner {
    grid-template-columns: 1fr;
    gap: 15px;
  }

  .submenu-column {
    padding: 10px;
    background: white;
    border-radius: 10px;
  }
}
</style>