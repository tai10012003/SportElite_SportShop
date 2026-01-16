<script setup>
import { ref, computed, onMounted, watch } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import AuthService from '@/services/AuthService'
import { useCartStore } from '@/stores/cart'

const router = useRouter()
const route = useRoute()
const user = ref(null)
const isLoggedIn = computed(() => !!user.value)
const username = computed(() => user.value?.hoTen || '')
const cartStore = useCartStore()

const cartCount = computed(() => {
  return Object.values(cartStore.items).reduce((sum, item) => sum + item.quantity, 0)
})

const isSidebarOpen = ref(false)
const activeSubmenu = ref(null)
const hoverMenu = ref(false)

const categories = [
  {
    title: 'Giày thể thao',
    items: [
      { name: 'Giày chạy bộ', link: '/san-pham/giay-chay-bo' },
      { name: 'Giày bóng đá', link: '/san-pham/giay-bong-da' },
      { name: 'Giày tennis', link: '/san-pham/giay-tennis' },
      { name: 'Giày bóng rổ', link: '/san-pham/giay-bong-ro' }
    ]
  },
  {
    title: 'Quần áo',
    items: [
      { name: 'Áo thể thao', link: '/san-pham/ao-the-thao' },
      { name: 'Quần thể thao', link: '/san-pham/quan-the-thao' },
      { name: 'Áo khoác', link: '/san-pham/ao-khoac' },
      { name: 'Bộ đồ tập', link: '/san-pham/bo-do-tap' }
    ]
  },
  {
    title: 'Phụ kiện',
    items: [
      { name: 'Balo thể thao', link: '/san-pham/balo-the-thao' },
      { name: 'Tất thể thao', link: '/san-pham/tat-the-thao' },
      { name: 'Băng đô', link: '/san-pham/bang-do' },
      { name: 'Găng tay', link: '/san-pham/gang-tay' }
    ]
  },
  {
    title: 'Dụng cụ tập luyện',
    items: [
      { name: 'Tạ tay', link: '/san-pham/ta-tay' },
      { name: 'Thảm tập yoga', link: '/san-pham/tham-tap-yoga' },
      { name: 'Dây nhảy', link: '/san-pham/day-nhay' },
      { name: 'Bóng tập', link: '/san-pham/bong-tap' }
    ]
  }
]

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
  closeSidebar()
  router.push('/dang-nhap')
}

function toggleSidebar() {
  isSidebarOpen.value = !isSidebarOpen.value
  if (!isSidebarOpen.value) {
    activeSubmenu.value = null
  }
}

function toggleSubmenu(index) {
  activeSubmenu.value = activeSubmenu.value === index ? null : index
}

function closeSidebar() {
  isSidebarOpen.value = false
  activeSubmenu.value = null
}

function isActive(path) {
  if (path === '/') {
    return route.path === '/'
  }
  return route.path === path || route.path.startsWith(path + '/')
}
</script>

<template>
  <nav class="navbar navbar-expand-lg navbar-light fixed-top">
    <div class="container">
      <router-link class="navbar-brand" to="/" @click="closeSidebar">
        <span class="gradient-text">Sport</span><strong>Elite</strong>
      </router-link>
      <div class="mobile-actions d-lg-none">
        <router-link class="mobile-cart-btn" to="/gio-hang" :class="{ active: isActive('/gio-hang') }">
          <i class="bi bi-cart3"></i>
          <span class="cart-badge" v-if="cartCount > 0">{{ cartCount }}</span>
        </router-link>
        <button class="mobile-toggle" @click="toggleSidebar" :class="{ active: isSidebarOpen }">
          <span></span>
          <span></span>
          <span></span>
        </button>
      </div>
      <div class="collapse navbar-collapse d-none d-lg-block" id="navbarNav">
        <ul class="navbar-nav ms-auto">
          <li class="nav-item">
            <router-link class="nav-link btn-hover px-4" to="/" :class="{ active: isActive('/') }">
              <i class="bi bi-house-door-fill me-2"></i>Trang chủ
            </router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link btn-hover px-4" to="/gioi-thieu" :class="{ active: isActive('/gioi-thieu') }">
              <i class="bi bi-info-circle-fill me-2"></i>Giới thiệu
            </router-link>
          </li>
          <li class="nav-item dropdown">
            <router-link class="nav-link btn-hover px-4 dropdown-toggle" to="/san-pham" :class="{ active: isActive('/san-pham') }">
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
            <router-link class="nav-link btn-hover px-4" to="/bai-viet" :class="{ active: isActive('/bai-viet') }">
              <i class="bi bi-newspaper me-2"></i>Bài viết
            </router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link btn-hover px-4" to="/lien-he" :class="{ active: isActive('/lien-he') }">
              <i class="bi bi-envelope-fill me-2"></i>Liên hệ
            </router-link>
          </li>
          <li class="nav-item nav-item-action ms-lg-4">
            <router-link class="nav-link cart-btn" to="/gio-hang" :class="{ active: isActive('/gio-hang') }">
              <i class="bi bi-cart3"></i>
              <span class="cart-badge">{{ cartCount }}</span>
            </router-link>
          </li>
          <li class="nav-item nav-item-action ms-lg-3">
            <div v-if="isLoggedIn" class="dropdown">
              <a class="nav-link user-menu" href="#" :class="{ active: isActive('/tai-khoan') || isActive('/san-pham-yeu-thich') || isActive('/don-hang-cua-toi')}">
                <i class="bi bi-person-circle me-2"></i>
                <span>{{ username }}</span>
              </a>
              <ul class="dropdown-menu dropdown-menu-end user-dropdown">
                <li>
                  <router-link class="dropdown-item" :class="{ active: isActive('/tai-khoan') }" to="/tai-khoan">
                    <i class="bi bi-person me-2"></i>Tài khoản
                  </router-link>
                </li>
                <li>
                  <router-link class="dropdown-item" :class="{ active: isActive('/san-pham-yeu-thich') }" to="/san-pham-yeu-thich">
                    <i class="bi bi-heart me-2"></i>Sản phẩm yêu thích
                  </router-link>
                </li>
                <li>
                  <router-link class="dropdown-item" :class="{ active: isActive('/don-hang-cua-toi') }" to="/don-hang-cua-toi">
                    <i class="bi bi-bag me-2"></i>Đơn hàng của tôi
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
  <div class="sidebar-overlay" :class="{ active: isSidebarOpen }" @click="closeSidebar"></div>
  <div class="mobile-sidebar" :class="{ active: isSidebarOpen }">
    <div class="sidebar-header">
      <div class="sidebar-brand">
        <span class="gradient-text">Sport</span><strong>Elite</strong>
      </div>
      <button class="sidebar-close" @click="closeSidebar">
        <i class="bi bi-x-lg"></i>
      </button>''
    </div>
    <div class="sidebar-content">
      <div class="sidebar-user" v-if="isLoggedIn">
        <div class="user-avatar">
          <i class="bi bi-person-circle"></i>
        </div>
        <div class="user-info">
          <h6>{{ username }}</h6>
          <p>Thành viên</p>
        </div>
      </div>
      <ul class="sidebar-menu">
        <li>
          <router-link to="/" :class="{ active: isActive('/') }" @click="closeSidebar">
            <i class="bi bi-house-door-fill"></i>
            <span>Trang chủ</span>
          </router-link>
        </li>
        <li>
          <router-link to="/gioi-thieu" :class="{ active: isActive('/gioi-thieu') }" @click="closeSidebar">
            <i class="bi bi-info-circle-fill"></i>
            <span>Giới thiệu</span>
          </router-link>
        </li>
        <li class="has-submenu">
          <a href="#" :class="{ active: isActive('/san-pham'), expanded: activeSubmenu === 'products' }" @click.prevent="toggleSubmenu('products')">
            <i class="bi bi-grid-fill"></i>
            <span>Sản phẩm</span>
            <i class="bi bi-chevron-down arrow"></i>
          </a>
          <ul class="sidebar-submenu" :class="{ show: activeSubmenu === 'products' }">
            <li v-for="(category, index) in categories" :key="index" class="submenu-category">
              <h6 class="category-title">{{ category.title }}</h6>
              <ul class="category-items">
                <li v-for="(item, idx) in category.items" :key="idx">
                  <router-link :to="item.link" @click="closeSidebar">
                    {{ item.name }}
                  </router-link>
                </li>
              </ul>
            </li>
          </ul>
        </li>
        <li>
          <router-link to="/bai-viet" :class="{ active: isActive('/bai-viet') }" @click="closeSidebar">
            <i class="bi bi-newspaper"></i>
            <span>Bài viết</span>
          </router-link>
        </li>
        <li>
          <router-link to="/lien-he" :class="{ active: isActive('/lien-he') }" @click="closeSidebar">
            <i class="bi bi-envelope-fill"></i>
            <span>Liên hệ</span>
          </router-link>
        </li>
      </ul>
      <div class="sidebar-divider" v-if="isLoggedIn"></div>
      <ul class="sidebar-menu" v-if="isLoggedIn">
        <li>
          <router-link to="/tai-khoan" :class="{ active: isActive('/tai-khoan') }" @click="closeSidebar">
            <i class="bi bi-person"></i>
            <span>Tài khoản</span>
          </router-link>
        </li>
        <li>
          <router-link to="/san-pham-yeu-thich" :class="{ active: isActive('/san-pham-yeu-thich') }" @click="closeSidebar">
            <i class="bi bi-heart"></i>
            <span>Sản phẩm yêu thích</span>
          </router-link>
        </li>
        <li>
          <router-link to="/don-hang-cua-toi" :class="{ active: isActive('/don-hang-cua-toi') }" @click="closeSidebar">
            <i class="bi bi-bag"></i>
            <span>Đơn hàng của tôi</span>
          </router-link>
        </li>
        <li>
          <a href="#" class="logout-link" @click.prevent="logout">
            <i class="bi bi-box-arrow-right"></i>
            <span>Đăng xuất</span>
          </a>
        </li>
      </ul>
      <div class="sidebar-login" v-else>
        <router-link to="/dang-nhap" class="btn-login-mobile" @click="closeSidebar">
          <i class="bi bi-person me-2"></i>Đăng nhập
        </router-link>
      </div>
    </div>
  </div>
</template>

<style scoped>
.navbar {
  background: rgba(255, 255, 255, 0.95) !important;
  backdrop-filter: blur(10px);
  box-shadow: 0 2px 20px rgba(0,0,0,0.05);
  padding: 1.2rem 0;
  transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
  z-index: 1000;
}

.navbar-brand {
  font-size: 1.8rem;
  font-weight: 700;
  padding: 0;
  margin: 0;
  line-height: 1;
}

.navbar-brand .gradient-text {
  background: linear-gradient(120deg, #367cd7, #34a853);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
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

.nav-link.active {
  color: #003780 !important;
  font-weight: 600;
  background: linear-gradient(90deg, rgba(26,115,232,0.15), rgba(52,168,83,0.15));
}

.user-menu.active,
.cart-btn.active {
  background: #003780;
  color: white !important;
  transform: translateY(-2px);
}

.user-menu.active i,
.cart-btn.active i {
  color: white;
}

.cart-btn.active .cart-badge {
  background: white;
  color: #003780;
  border-color: #003780;
}

.nav-item.dropdown {
  position: static;
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

.login-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(0,0,0,0.15);
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

.user-menu:hover {
  background: #003780;
  color: white !important;
  transform: translateY(-2px);
}

.user-dropdown {
  min-width: 220px;
  padding: 0.5rem;
  border: none;
  border-radius: 15px;
  box-shadow: 0 10px 30px rgba(0,0,0,0.1);
}

.user-dropdown .dropdown-item {
  padding: 0.7rem 1rem;
  border-radius: 10px;
  margin-top: 5px;
  display: flex;
  align-items: center;
  font-size: 0.95rem;
  transition: all 0.3s ease;
  color: #202124;
}

.user-dropdown .dropdown-item:hover {
  background: #eeeeee;
  color: #003780;
}

.user-dropdown .dropdown-item i {
  font-size: 1.1rem;
  width: 24px;
}

.nav-item-action .dropdown:hover .dropdown-menu {
  display: block;
  opacity: 1;
  transition: opacity 0.2s ease-in-out;
}

.dropdown-item.active, .dropdown-item:active {
  background: #eeeeee;
  color: #003780;
  font-weight: 700;
  text-decoration: none;
}

.text-danger {
  color: #dc3545 !important;
}

.text-danger:hover {
  color: #dc3545 !important;
  background: rgba(220, 53, 69, 0.1) !important;
}

.mobile-actions {
  display: flex;
  align-items: center;
  gap: 15px;
}

.mobile-cart-btn {
  position: relative;
  width: 46px;
  height: 42px;
  background: #f8f9fa;
  border: 1.5px solid #003780;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #003780;
  transition: all 0.3s ease;
}

.mobile-cart-btn.active {
  background: #003780;
  color: white !important;
  transform: translateY(-2px);
}

.mobile-cart-btn i {
  font-size: 1.3rem;
}

.mobile-cart-btn .cart-badge {
  position: absolute;
  top: -6px;
  right: -6px;
  background: #003780;
  color: white;
  font-size: 0.7rem;
  min-width: 18px;
  height: 18px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  border: 2px solid white;
}

.mobile-toggle {
  width: 42px;
  height: 42px;
  background: #f8f9fa;
  border: 1.5px solid #003780;
  border-radius: 10px;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  gap: 5px;
  padding: 0;
  cursor: pointer;
  transition: all 0.3s ease;
}

.mobile-toggle span {
  width: 22px;
  height: 2.5px;
  background: #003780;
  border-radius: 2px;
  transition: all 0.3s ease;
}

.mobile-toggle.active span:nth-child(1) {
  transform: rotate(45deg) translateY(7.5px);
}

.mobile-toggle.active span:nth-child(2) {
  opacity: 0;
}

.mobile-toggle.active span:nth-child(3) {
  transform: rotate(-45deg) translateY(-7.5px);
}

.sidebar-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  backdrop-filter: blur(5px);
  z-index: 998;
  opacity: 0;
  visibility: hidden;
  transition: all 0.3s ease;
}

.sidebar-overlay.active {
  opacity: 1;
  visibility: visible;
}

.mobile-sidebar {
  position: fixed;
  top: 0;
  left: -100%;
  width: 270px;
  max-width: 85%;
  height: 100%;
  background: white;
  z-index: 999;
  transition: left 0.4s cubic-bezier(0.4, 0, 0.2, 1);
  box-shadow: -5px 0 30px rgba(0, 0, 0, 0.1);
  display: flex;
  flex-direction: column;
}

.mobile-sidebar.active {
  left: 0;
}

.sidebar-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 20px 25px;
  border-bottom: 1px solid #f0f0f0;
  background: linear-gradient(135deg, rgba(54, 124, 215, 0.05), rgba(52, 168, 83, 0.05));
}

.sidebar-brand {
  font-size: 1.5rem;
  font-weight: 700;
}

.sidebar-brand .gradient-text {
  background: linear-gradient(120deg, #367cd7, #34a853);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}

.sidebar-brand strong {
  color: #003780;
}

.sidebar-close {
  width: 36px;
  height: 36px;
  border: none;
  background: rgba(0, 55, 128, 0.1);
  border-radius: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #003780;
  font-size: 1.2rem;
  cursor: pointer;
  transition: all 0.3s ease;
}

.sidebar-close:hover {
  background: #003780;
  color: white;
}

.sidebar-content {
  flex: 1;
  overflow-y: auto;
  padding: 20px 0;
}

.sidebar-user {
  display: flex;
  align-items: center;
  gap: 15px;
  padding: 20px 25px;
  margin-bottom: 10px;
  background: linear-gradient(135deg, rgba(54, 124, 215, 0.08), rgba(52, 168, 83, 0.08));
  border-radius: 15px;
  margin: 0 20px 20px;
}

.user-avatar {
  width: 50px;
  height: 50px;
  background: linear-gradient(135deg, #367cd7, #34a853);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 1.8rem;
}

.user-info h6 {
  margin: 0;
  font-size: 1rem;
  font-weight: 600;
  color: #003780;
}

.user-info p {
  margin: 0;
  font-size: 0.85rem;
  color: #666;
}

.sidebar-menu {
  list-style: none;
  padding: 0;
  margin: 0 20px;
}

.sidebar-menu li {
  margin-bottom: 5px;
}

.sidebar-menu a {
  display: flex;
  align-items: center;
  padding: 14px 15px;
  color: #333;
  text-decoration: none;
  border-radius: 12px;
  transition: all 0.3s ease;
  font-weight: 500;
  position: relative;
}

.sidebar-menu a i {
  width: 24px;
  font-size: 1.2rem;
  margin-right: 12px;
  color: #003780;
}

.sidebar-menu a span {
  flex: 1;
}

.sidebar-menu a:hover {
  background: rgba(54, 124, 215, 0.08);
  color: #003780;
  transform: translateX(5px);
}

.sidebar-menu a.active {
  background: linear-gradient(135deg, rgba(54, 124, 215, 0.15), rgba(52, 168, 83, 0.15));
  color: #003780;
  font-weight: 600;
}

.sidebar-menu a.active::before {
  content: '';
  position: absolute;
  left: 0;
  top: 50%;
  transform: translateY(-50%);
  width: 4px;
  height: 70%;
  background: linear-gradient(135deg, #367cd7, #34a853);
  border-radius: 0 4px 4px 0;
}

.has-submenu > a .arrow {
  margin-left: auto;
  transition: transform 0.3s ease;
  font-size: 0.9rem;
}

.has-submenu > a.expanded .arrow {
  transform: rotate(180deg);
}

.sidebar-submenu {
  max-height: 0;
  overflow: hidden;
  transition: max-height 0.4s cubic-bezier(0.4, 0, 0.2, 1);
  padding-left: 15px;
  margin-top: 5px;
}

.sidebar-submenu.show {
  max-height: 1000px;
}

.submenu-category {
  margin-bottom: 20px;
  list-style-type: none;
}

.category-title {
  font-size: 0.85rem;
  font-weight: 600;
  color: #003780;
  margin-bottom: 10px;
  padding: 10px;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.category-items {
  list-style: none;
  padding: 0;
  margin: 0;
}

.category-items li {
  margin-bottom: 3px;
}

.category-items a {
  display: block;
  padding: 10px 10px 10px 20px;
  color: #555;
  text-decoration: none;
  border-radius: 8px;
  transition: all 0.3s ease;
  font-size: 0.9rem;
  font-weight: 400;
  position: relative;
}

.category-items a::before {
  content: '•';
  position: absolute;
  left: 8px;
  color: #34a853;
}

.category-items a:hover {
  background: rgba(52, 168, 83, 0.1);
  color: #34a853;
  transform: translateX(3px);
}

.sidebar-divider {
  height: 1px;
  background: linear-gradient(90deg, transparent, #e0e0e0, transparent);
  margin: 20px 20px;
}

.logout-link {
  color: #dc3545 !important;
}

.logout-link:hover {
  background: rgba(220, 53, 69, 0.1) !important;
  color: #dc3545 !important;
}

.sidebar-login {
  padding: 20px 25px;
  margin-top: auto;
}

.btn-login-mobile {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100%;
  padding: 14px 20px;
  background: linear-gradient(120deg, #367cd7, #34a853);
  color: white;
  text-decoration: none;
  border-radius: 12px;
  font-weight: 600;
  font-size: 1rem;
  box-shadow: 0 4px 15px rgba(54, 124, 215, 0.3);
  transition: all 0.3s ease;
}

.btn-login-mobile:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(54, 124, 215, 0.4);
  color: white;
}

.sidebar-content::-webkit-scrollbar {
  width: 6px;
}

.sidebar-content::-webkit-scrollbar-track {
  background: #f1f1f1;
}

.sidebar-content::-webkit-scrollbar-thumb {
  background: linear-gradient(135deg, #367cd7, #34a853);
  border-radius: 10px;
}

.sidebar-content::-webkit-scrollbar-thumb:hover {
  background: linear-gradient(135deg, #2d5fb8, #2d8f44);
}

@media (max-width: 991px) {
  .navbar {
    padding: 0.8rem 0;
  }

  .navbar-brand {
    font-size: 1.5rem;
  }

  .submenu {
    display: none !important;
  }
}

@media (min-width: 992px) {
  .mobile-actions,
  .sidebar-overlay,
  .mobile-sidebar {
    display: none !important;
  }
}

@media (max-width: 576px) {
  .sidebar-header {
    padding: 18px 20px;
  }

  .sidebar-brand {
    font-size: 1.4rem;
  }

  .sidebar-menu {
    margin: 0 15px;
  }

  .sidebar-user {
    margin: 0 15px 20px;
    padding: 18px 20px;
  }

  .user-avatar {
    width: 45px;
    height: 45px;
    font-size: 1.6rem;
  }

  .user-info h6 {
    font-size: 0.95rem;
  }

  .user-info p {
    font-size: 0.8rem;
  }
}

@keyframes slideInRight {
  from {
    opacity: 0;
    transform: translateX(20px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

.mobile-sidebar.active .sidebar-menu > li {
  animation: slideInRight 0.3s ease forwards;
}

.mobile-sidebar.active .sidebar-menu > li:nth-child(1) { animation-delay: 0.05s; }
.mobile-sidebar.active .sidebar-menu > li:nth-child(2) { animation-delay: 0.1s; }
.mobile-sidebar.active .sidebar-menu > li:nth-child(3) { animation-delay: 0.15s; }
.mobile-sidebar.active .sidebar-menu > li:nth-child(4) { animation-delay: 0.2s; }
.mobile-sidebar.active .sidebar-menu > li:nth-child(5) { animation-delay: 0.25s; }
.mobile-sidebar.active .sidebar-menu > li:nth-child(6) { animation-delay: 0.3s; }
</style>