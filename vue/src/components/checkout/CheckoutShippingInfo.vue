<template>
  <div class="checkout-section mb-4">
    <div class="card shadow-sm border-0">
      <div class="card-header bg-white">
        <h5 class="card-title mb-0 py-2">
          <i class="bi bi-person-lines-fill me-2"></i>THÔNG TIN NHẬN HÀNG
        </h5>
      </div>
      <div class="card-body">
        <form class="checkout-form">
          <div class="row g-3">
            <div class="col-md-12">
              <div class="form-group">
                <label class="form-label" for="receiver_name">Tên người nhận</label>
                <input 
                  type="text" 
                  class="form-control" 
                  id="receiver_name" 
                  v-model="formData.receiverName"
                  placeholder="Nhập tên người nhận"
                  required
                >
              </div>
            </div>
            <div class="col-md-12">
              <div class="form-group">
                <label class="form-label" for="receiver_phone">Số điện thoại</label>
                <input 
                  type="tel" 
                  class="form-control" 
                  id="receiver_phone" 
                  v-model="formData.receiverPhone"
                  placeholder="Nhập số điện thoại"
                  required
                >
              </div>
            </div>
            <div class="col-12">
              <div class="form-group">
                <label class="form-label" for="province_select">Tỉnh/Thành phố</label>
                <select 
                  class="form-select" 
                  id="province_select" 
                  v-model="formData.province"
                  @change="onProvinceChange"
                  required
                >
                  <option value="">Chọn tỉnh/thành phố</option>
                  <option 
                    v-for="province in provinces" 
                    :key="province.code" 
                    :value="province.code"
                  >
                    {{ province.name_with_type }}
                  </option>
                </select>
              </div>
            </div>
            <div class="col-12">
              <div class="form-group">
                <label class="form-label" for="district_select">Quận/Huyện/Thị xã</label>
                <select 
                  class="form-select" 
                  id="district_select" 
                  v-model="formData.district"
                  @change="onDistrictChange"
                  :disabled="!formData.province"
                  required
                >
                  <option value="">Chọn quận/huyện</option>
                  <option 
                    v-for="district in districts" 
                    :key="district.code" 
                    :value="district.code"
                  >
                    {{ district.name_with_type }}
                  </option>
                </select>
              </div>
            </div>
            <div class="col-12">
              <div class="form-group">
                <label class="form-label" for="ward_select">Phường/Xã/Thị trấn</label>
                <select 
                  class="form-select" 
                  id="ward_select" 
                  v-model="formData.ward"
                  :disabled="!formData.district"
                  required
                >
                  <option value="">Chọn phường/xã</option>
                  <option 
                    v-for="ward in wards" 
                    :key="ward.code" 
                    :value="ward.code"
                  >
                    {{ ward.name_with_type }}
                  </option>
                </select>
              </div>
            </div>
            <div class="col-12">
              <div class="form-group">
                <label class="form-label" for="specific_address">Địa chỉ cụ thể (Chỉ viết "Số nhà/số căn hộ (kèm ngõ/hẻm nếu có) + Tên đường")</label>
                <input 
                  type="text"
                  class="form-control" 
                  id="specific_address" 
                  v-model="formData.specificAddress"
                  placeholder="VD: 123/5A, đường Nguyễn Văn Linh"
                  required
                >
              </div>
            </div>
            <div class="col-12" v-if="fullAddress">
              <div class="form-group">
                <label class="form-label">Địa chỉ đầy đủ</label>
                <div class="full-address-display">
                  <i class="bi bi-geo-alt-fill me-2"></i>
                  {{ fullAddress }}
                </div>
              </div>
            </div>
            <div class="col-12">
              <div class="form-group">
                <label class="form-label" for="order_note">Ghi chú đơn hàng (nếu có)</label>
                <textarea 
                  class="form-control" 
                  id="order_note" 
                  rows="3"
                  v-model="formData.orderNote"
                  placeholder="Nhập ghi chú cho đơn hàng (nếu có)"
                ></textarea>
              </div>
            </div>
          </div>
        </form>
      </div>
    </div>
  </div>
  <div class="checkout-section">
    <div class="card shadow-sm border-0">
      <div class="card-header bg-white">
        <h5 class="card-title mb-0 py-2">
          <i class="bi bi-credit-card me-2"></i>PHƯƠNG THỨC THANH TOÁN
        </h5>
      </div>
      <div class="card-body">
        <div class="payment-methods">
          <div class="payment-method">
            <input 
              type="radio" 
              class="btn-check" 
              name="payment_method" 
              id="cod" 
              value="cod" 
              v-model="formData.paymentMethod"
            >
            <label class="btn btn-outline-payment w-100 text-start mb-3" for="cod">
              <i class="bi bi-cash-coin me-2"></i>
              <span>Thanh toán khi nhận hàng (COD)</span>
            </label>
          </div>
          <div class="payment-method">
            <input 
              type="radio" 
              class="btn-check" 
              name="payment_method" 
              id="vnpay" 
              value="vnpay"
              v-model="formData.paymentMethod"
            >
            <label class="btn btn-outline-payment w-100 text-start mb-3" for="vnpay">
              <i class="bi bi-wallet2 me-2"></i>
              <span>Thanh toán VNPay</span>
            </label>
          </div>
          <div class="payment-method">
            <input 
              type="radio" 
              class="btn-check" 
              name="payment_method" 
              id="momo" 
              value="momo"
              v-model="formData.paymentMethod"
            >
            <label class="btn btn-outline-payment w-100 text-start" for="momo">
              <i class="bi bi-wallet2 me-2"></i>
              <span>Thanh toán qua MoMo</span>
            </label>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { reactive, onMounted, watch, computed } from 'vue'
import UserService from '@/services/UserService'

const props = defineProps({
  modelValue: {
    type: Object,
    required: true
  }
})

const emit = defineEmits(['update:modelValue'])

const formData = reactive({
  receiverName: '',
  receiverPhone: '',
  province: '',
  district: '',
  ward: '',
  specificAddress: '',
  orderNote: '',
  paymentMethod: 'cod'
})

const provinces = reactive([])
const districts = reactive([])
const wards = reactive([])

const fullAddress = computed(() => {
  const parts = []
  if (formData.specificAddress) {
    parts.push(formData.specificAddress.replace(/,/g, ''))
  }
  const wardName = wards.find(w => w.code === formData.ward)?.name_with_type
  if (wardName) parts.push(wardName)
  const districtName = districts.find(d => d.code === formData.district)?.name_with_type
  if (districtName) parts.push(districtName)
  const provinceName = provinces.find(p => p.code === formData.province)?.name_with_type
  if (provinceName) parts.push(provinceName)
  return parts.length > 0 ? parts.join(',') : ''
})

const parseAddress = (addressString) => {
  if (!addressString) return { specificAddress: '', ward: '', district: '', province: '' }
  const parts = addressString.split(',').map(p => p.trim())
  return {
    specificAddress: parts[0] || '',
    ward: parts[1] || '',
    district: parts[2] || '',
    province: parts[3] || ''
  }
}

const findProvinceCode = (provinceName) => {
  const province = provinces.find(p => 
    p.name_with_type === provinceName || 
    p.name === provinceName
  )
  return province?.code || ''
}

const findDistrictCode = (districtName) => {
  const district = districts.find(d => 
    d.name_with_type === districtName || 
    d.name === districtName
  )
  return district?.code || ''
}

const findWardCode = (wardName) => {
  const ward = wards.find(w => 
    w.name_with_type === wardName || 
    w.name === wardName
  )
  return ward?.code || ''
}

const loadUserProfile = async () => {
  try {
    const userData = await UserService.getUserProfile()
    formData.receiverName = userData.hoTen || ''
    formData.receiverPhone = userData.soDienThoai || ''
    if (userData.diaChi) {
      const parsed = parseAddress(userData.diaChi)
      formData.specificAddress = parsed.specificAddress
      if (parsed.province) {
        const provinceCode = findProvinceCode(parsed.province)
        if (provinceCode) {
          formData.province = provinceCode
          await onProvinceChange(false)
          if (parsed.district) {
            const districtCode = findDistrictCode(parsed.district)
            if (districtCode) {
              formData.district = districtCode
              await onDistrictChange(false)
              if (parsed.ward) {
                const wardCode = findWardCode(parsed.ward)
                if (wardCode) {
                  formData.ward = wardCode
                }
              }
            }
          }
        }
      }
    }
  } catch (error) {
    console.error('Error loading user profile:', error)
  }
}

const loadProvinces = async () => {
  try {
    const response = await fetch('https://vn-public-apis.fpo.vn/provinces/getAll?limit=-1')
    const data = await response.json()
    if (data.exitcode === 1) {
      provinces.push(...data.data.data)
    }
  } catch (error) {
    console.error('Error loading provinces:', error)
  }
}

const onProvinceChange = async (shouldReset = true) => {
  if (shouldReset) {
    formData.district = ''
    formData.ward = ''
  }
  districts.length = 0
  wards.length = 0
  if (formData.province) {
    try {
      const response = await fetch(`https://vn-public-apis.fpo.vn/districts/getByProvince?provinceCode=${formData.province}&limit=-1`)
      const data = await response.json()
      if (data.exitcode === 1) {
        districts.push(...data.data.data)
      }
    } catch (error) {
      console.error('Error loading districts:', error)
    }
  }
}

const onDistrictChange = async (shouldReset = true) => {
  if (shouldReset) {
    formData.ward = ''
  }
  wards.length = 0
  if (formData.district) {
    try {
      const response = await fetch(`https://vn-public-apis.fpo.vn/wards/getByDistrict?districtCode=${formData.district}&limit=-1`)
      const data = await response.json()
      if (data.exitcode === 1) {
        wards.push(...data.data.data)
      }
    } catch (error) {
      console.error('Error loading wards:', error)
    }
  }
}

watch(formData, (newValue) => {
  emit('update:modelValue', { ...newValue, fullAddress: fullAddress.value })
}, { deep: true })

onMounted(async () => {
  await loadProvinces()
  await new Promise(resolve => setTimeout(resolve, 100))
  await loadUserProfile()
})
</script>

<style scoped>
.checkout-section {
  margin-bottom: 1.5rem;
}

.card {
  border-radius: 8px;
  overflow: hidden;
}

.card-header {
  padding: 1rem 1.25rem;
  border-bottom: 2px solid #f0f0f0;
}

.card-title {
  font-size: 1.2rem;
  font-weight: 700;
  color: #333;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  display: flex;
  align-items: center;
}

.card-title i {
  font-size: 1.2rem;
  color: #666;
}

.card-body {
  padding: 1.5rem 1.25rem;
}

.form-group {
  margin-bottom: 0.8rem;
}

.form-label {
  font-weight: 500;
  color: #333;
  margin-bottom: 0.5rem;
  font-size: 1rem;
}

.form-control,
.form-select {
  border: 1px solid #ddd;
  border-radius: 6px;
  padding: 0.65rem 0.875rem;
  font-size: 1rem;
  transition: all 0.3s ease;
}

.form-control:focus,
.form-select:focus {
  border-color: #3b82f6;
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
  outline: none;
}

.form-control::placeholder {
  color: #aaa;
}

textarea.form-control {
  resize: vertical;
  min-height: 90px;
}

.full-address-display {
  padding: 0.75rem 1rem;
  background: #f0f7ff;
  border: 1px solid #d0e7ff;
  border-radius: 6px;
  color: #1e40af;
  font-weight: 500;
  display: flex;
  align-items: center;
  font-size: 0.95rem;
}

.full-address-display i {
  color: #3b82f6;
  font-size: 1.1rem;
}

.form-select:disabled {
  background-color: #f5f5f5;
  cursor: not-allowed;
  opacity: 0.6;
}

.payment-methods {
  display: flex;
  flex-direction: column;
  gap: 0;
}

.btn-check {
  display: none;
}

.btn-outline-payment {
  padding: 0.875rem 1rem;
  border: 2px solid #e8e8e8;
  border-radius: 8px;
  background: white;
  color: #333;
  font-weight: 500;
  font-size: 1rem;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
}

.btn-outline-payment i {
  font-size: 1.3rem;
  color: #666;
}

.btn-outline-payment:hover {
  border-color: #3b82f6 !important;
  background: #f0f7ff;
}

.btn-check:checked + .btn-outline-payment {
  border-color: #3b82f6;
  background: #3b82f6;
  color: white;
  box-shadow: 0 2px 8px rgba(59, 130, 246, 0.3);
}

.btn-check:checked + .btn-outline-payment i {
  color: white;
}

@media (max-width: 768px) {
  .card-header {
    padding: 0.875rem 1rem;
  }
  
  .card-title {
    font-size: 0.95rem;
  }
  
  .card-body {
    padding: 1.25rem 1rem;
  }
  
  .form-label {
    font-size: 0.85rem;
  }
  
  .form-control,
  .form-select {
    font-size: 0.85rem;
    padding: 0.6rem 0.75rem;
  }
  
  .btn-outline-payment {
    padding: 0.75rem 0.875rem;
    font-size: 0.85rem;
  }
  
  .btn-outline-payment i {
    font-size: 1.15rem;
  }
}

@media (max-width: 576px) {
  .checkout-section {
    margin-bottom: 1.25rem;
  }
  
  .card {
    border-radius: 6px;
  }
  
  .card-header {
    padding: 0.75rem 0.875rem;
  }
  
  .card-title {
    font-size: 0.875rem;
  }
  
  .card-title i {
    font-size: 1rem;
  }
  
  .card-body {
    padding: 1rem 0.875rem;
  }
  
  .form-group {
    margin-bottom: 0;
  }
  
  textarea.form-control {
    min-height: 80px;
  }

  .full-address-display {
    font-size: 0.85rem;
    padding: 0.65rem 0.875rem;
  }
  
  .full-address-display i {
    font-size: 1rem;
  }
  
  .btn-outline-payment {
    padding: 0.7rem 0.8rem;
    font-size: 0.8rem;
  }
}
</style>