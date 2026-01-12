<template>
  <div class="checkout-page">
    <div class="container">
      <nav aria-label="breadcrumb-cart">
        <ol class="breadcrumb">
          <li class="breadcrumb-item">
            <router-link to="/">Trang chủ</router-link>
          </li>
          <li class="breadcrumb-item">
            <router-link to="/gio-hang">Giỏ hàng</router-link>
          </li>
          <li class="breadcrumb-item">
            <router-link to="/thanh-toan">Thanh toán</router-link>
          </li>
        </ol>
      </nav>
      <div class="row">
        <div class="col-lg-7">
          <CheckoutShippingInfo 
            :model-value="shippingData" 
            @update:model-value="handleShippingUpdate" 
          />
        </div>
        <div class="col-lg-5">
          <CheckoutOrderSummary @place-order="handlePlaceOrder" />
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import Swal from 'sweetalert2'
import { useCartStore } from '@/stores/cart'
import CheckoutShippingInfo from '@/components/checkout/CheckoutShippingInfo.vue'
import CheckoutOrderSummary from '@/components/checkout/CheckoutOrderSummary.vue'
import AuthService from '@/services/AuthService'
import OrderService from '@/services/OrderService'

const router = useRouter()
const cartStore = useCartStore()

const shippingData = reactive({
  receiverName: '',
  receiverPhone: '',
  province: '',
  district: '',
  ward: '',
  specificAddress: '',
  fullAddress: '',
  orderNote: '',
  paymentMethod: 'cod'
})

const handleShippingUpdate = (newData) => {
  Object.assign(shippingData, newData)
}

onMounted(async () => {
  if (!AuthService.isLoggedIn()) {
    await Swal.fire({
      icon: 'warning',
      title: 'Chưa đăng nhập',
      text: 'Vui lòng đăng nhập để tiếp tục thanh toán!'
    })
    router.push('/dang-nhap')
    return
  }
  if (cartStore.itemCount === 0) {
    await Swal.fire({
      icon: 'info',
      title: 'Giỏ hàng trống',
      text: 'Giỏ hàng của bạn hiện đang trống!'
    })
    router.push('/gio-hang')
    return
  }
})

const formatImagePath = (imageUrl) => {
  if (!imageUrl) return null
  if (imageUrl.startsWith('/assets/')) {
    return imageUrl
  }
  if (imageUrl.includes('/src/assets/')) {
    return imageUrl.replace(/^.*\/src(\/assets\/.+)$/, '$1')
  }
  if (imageUrl.includes('/assets/')) {
    return imageUrl.replace(/^.*?(\/assets\/.+)$/, '$1')
  }
  return imageUrl
}

const handlePlaceOrder = async (orderTotals) => {
  if (!shippingData.receiverName.trim() || !shippingData.receiverPhone.trim() || !shippingData.fullAddress.trim()) {
    await Swal.fire({
      icon: 'warning',
      title: 'Thiếu thông tin',
      text: 'Vui lòng điền đầy đủ thông tin nhận hàng!'
    })
    return
  }
  const phoneRegex = /^[0-9]{10,11}$/
  if (!phoneRegex.test(shippingData.receiverPhone.replace(/\s/g, ''))) {
    await Swal.fire({
      icon: 'error',
      title: 'Số điện thoại không hợp lệ',
      text: 'Vui lòng kiểm tra lại số điện thoại!'
    })
    return
  }
  try {
    const orderData = {
      tenNguoiNhan: shippingData.receiverName,
      soDienThoai: shippingData.receiverPhone,
      diaChi: shippingData.fullAddress,
      ghiChu: shippingData.orderNote || '',
      phuongThucThanhToan: shippingData.paymentMethod,
      maGiamGia: null,
      items: Object.values(cartStore.items).map(item => ({
        maSanPham: item.maSanPham,
        tenSanPham: item.name,
        hinhAnh: formatImagePath(item.image),
        soLuong: item.quantity,
        mauSac: item.color || null,
        kichThuoc: item.size || null,
        donGia: item.price
      }))
    }
    const result = await OrderService.createOrder(orderData)
    await Swal.fire({
      icon: 'success',
      title: 'Đặt hàng thành công! 🎉',
      html: `
        <p><strong>Mã đơn hàng:</strong> ${result.maDonHang}</p>
        <p>Cảm ơn bạn đã mua hàng tại Sport Elite!</p>
      `,
      confirmButtonText: 'Xem đơn hàng',
      showCancelButton: true,
      cancelButtonText: 'Tiếp tục mua sắm'
    }).then((sweetResult) => {
      cartStore.clearCart()
      if (sweetResult.isConfirmed) {
        router.push('/don-hang-cua-toi')
      } else {
        router.push('/san-pham')
      }
    })
  } catch (error) {
    console.error('Error placing order:', error)
    await Swal.fire({
      icon: 'error',
      title: 'Đặt hàng thất bại',
      text: error.message || 'Vui lòng thử lại sau!',
      confirmButtonText: 'Đóng'
    })
  }
}
</script>

<style scoped>
.checkout-page {
  padding: 30px;
  background: #f5f5f5;
}

.container {
  max-width: 1200px;
}

.breadcrumb {
  background: transparent;
  padding: 0.5rem 0;
}

.breadcrumb-item a {
  color: var(--primary-color);
  font-size: 0.95rem;
}

.row {
  margin: 0 -0.75rem;
}

.row > [class*='col-'] {
  padding: 0 0.75rem;
}

@media (max-width: 992px) {
  .checkout-page {
    padding: 10px;
  }

  .row {
    margin: 0 -0.5rem;
  }
  
  .row > [class*='col-'] {
    padding: 0 0.5rem;
  }
}

@media (max-width: 768px) {
  .checkout-page {
    padding: 10px;
  }

  .breadcrumb-checkout {
    font-size: 0.85rem;
  }
}

@media (max-width: 576px) {
  .checkout-page {
    padding: 10px;
  }

  .container {
    padding: 0.75rem;
  }
  
  .breadcrumb-checkout {
    font-size: 0.8rem;
    margin-bottom: 0.75rem !important;
  }
  
  .row {
    margin: 0 -0.375rem;
  }
  
  .row > [class*='col-'] {
    padding: 0 0.375rem;
  }
}
</style>