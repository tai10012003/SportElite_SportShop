import { createRouter, createWebHistory } from 'vue-router'
import Home from '@/views/Home.vue'
import About from '@/views/About.vue'
import Product from '@/views/Product.vue'
import Login from '@/views/Login.vue'
import Register from '@/views/Register.vue'
import Profile from '@/views/Profile.vue'
import AuthService from '@/services/AuthService'
import ProductDetail from '@/views/ProductDetail.vue'
import Cart from '@/views/Cart.vue'

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/gioi-thieu',
    name: 'About',
    component: About
  },
  {
    path: '/san-pham',
    name: 'Product',
    component: Product
  },
  {
    path: '/san-pham/:slug',
    name: 'ProductDetail',
    component: ProductDetail,
    props: true
  },
  {
    path: '/gio-hang',
    name: 'Cart',
    component: Cart
  },
  {
    path: '/dang-nhap',
    name: 'Login',
    component: Login
  },
  {
    path: '/dang-ky',
    name: 'Register',
    component: Register
  },
  {
    path: '/tai-khoan',
    name: 'Profile',
    component: Profile,
    meta: { requiresAuth: true }
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, from, next) => {
  if (to.matched.some(record => record.meta.requiresAuth)) {
    if (!AuthService.isLoggedIn()) {
      next('/dang-nhap')
    } else {
      next()
    }
  } else {
    next()
  }
})
export default router
