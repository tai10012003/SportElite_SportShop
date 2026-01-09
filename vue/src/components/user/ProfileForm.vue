<template>
  <div class="container py-5 profile-wrapper">
    <div class="row justify-content-center">
      <div class="col-lg-8">
        <div class="account-wrapper">
          <div class="account-content">
            <div class="account-section">
              <h5 class="section-titles">
                <i class="bi bi-person-circle me-2"></i>
                THÔNG TIN CÁ NHÂN
              </h5>
              <form class="account-form" @submit.prevent="handleSubmit">
                <div class="row">
                  <div class="col-md-6">
                    <div class="form-group">
                      <label class="form-label" for="ho_ten">Họ và tên</label>
                      <div class="input-group">
                        <span class="input-group-text">
                          <i class="bi bi-person"></i>
                        </span>
                        <input
                          type="text"
                          id="ho_ten"
                          class="form-control"
                          v-model.trim="form.hoTen"
                          :class="{ 'is-invalid': v$.hoTen.$error }"
                          placeholder="Nhập họ tên"
                        />
                        <div class="invalid-feedback" v-if="v$.hoTen.$error">
                          <span v-if="!v$.hoTen.required">Họ tên là bắt buộc</span>
                          <span v-else-if="!v$.hoTen.minLength">Họ tên phải ít nhất 3 ký tự</span>
                          <span v-else-if="!v$.hoTen.maxLength">Họ tên tối đa 40 ký tự</span>
                        </div>
                      </div>
                    </div>
                  </div>
                  <div class="col-md-6">
                    <div class="form-group">
                      <label class="form-label" for="email">Email</label>
                      <div class="input-group">
                        <span class="input-group-text">
                          <i class="bi bi-envelope"></i>
                        </span>
                        <input
                          type="email"
                          id="email"
                          class="form-control"
                          v-model.trim="form.email"
                          :class="{ 'is-invalid': v$.email.$error }"
                          placeholder="Nhập email"
                        />
                        <div class="invalid-feedback" v-if="v$.email.$error">
                          <span v-if="!v$.email.required">Email là bắt buộc</span>
                          <span v-else-if="!v$.email.email">Email không hợp lệ</span>
                          <span v-else-if="!v$.email.maxLength">Email tối đa 100 ký tự</span>
                        </div>
                      </div>
                    </div>
                  </div>
                  <div class="col-md-6">
                    <div class="form-group">
                      <label class="form-label" for="so_dien_thoai">Số điện thoại</label>
                      <div class="input-group">
                        <span class="input-group-text">
                          <i class="bi bi-phone"></i>
                        </span>
                        <input
                          type="tel"
                          id="so_dien_thoai"
                          class="form-control"
                          v-model.trim="form.soDienThoai"
                          :class="{ 'is-invalid': v$.soDienThoai.$error }"
                          placeholder="Nhập số điện thoại"
                        />
                        <div class="invalid-feedback" v-if="v$.soDienThoai.$error">
                          <span v-if="!v$.soDienThoai.maxLength">Số điện thoại tối đa 20 ký tự</span>
                          <span v-else>Định dạng số điện thoại không hợp lệ</span>
                        </div>
                      </div>
                    </div>
                  </div>
                  <div class="col-md-6">
                    <div class="form-group">
                      <label class="form-label" for="province_select">Tỉnh/Thành phố</label>
                      <div class="input-group">
                        <span class="input-group-text">
                          <i class="bi bi-map"></i>
                        </span>
                        <select 
                          class="form-select" 
                          id="province_select" 
                          v-model="form.province"
                          @change="onProvinceChange()"
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
                  </div>
                  <div class="col-md-6">
                    <div class="form-group">
                      <label class="form-label" for="district_select">Quận/Huyện/Thị xã</label>
                      <div class="input-group">
                        <span class="input-group-text">
                          <i class="bi bi-building"></i>
                        </span>
                        <select 
                          class="form-select" 
                          id="district_select" 
                          v-model="form.district"
                          @change="onDistrictChange()"
                          :disabled="!form.province"
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
                  </div>
                  <div class="col-md-6">
                    <div class="form-group">
                      <label class="form-label" for="ward_select">Phường/Xã/Thị trấn</label>
                      <div class="input-group">
                        <span class="input-group-text">
                          <i class="bi bi-house"></i>
                        </span>
                        <select 
                          class="form-select" 
                          id="ward_select" 
                          v-model="form.ward"
                          :disabled="!form.district"
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
                  </div>
                  <div class="col-md-6">
                    <div class="form-group">
                      <label class="form-label" for="specific_address">Địa chỉ cụ thể (Chỉ viết "Số nhà/số căn hộ (kèm ngõ/hẻm nếu có) + Tên đường")</label>
                      <div class="input-group">
                        <span class="input-group-text">
                          <i class="bi bi-geo-alt"></i>
                        </span>
                        <input 
                          type="text"
                          class="form-control" 
                          id="specific_address" 
                          v-model.trim="form.specificAddress"
                          placeholder="VD: 123/5A, đường Nguyễn Văn Linh"
                        >
                      </div>
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
                    <button type="submit" class="btn btn-primary btn-update" :disabled="loading">
                      <span v-if="loading" class="spinner-border spinner-border-sm me-2"></span>
                      <i v-else class="bi bi-check-circle me-2"></i>
                      Cập nhật thông tin
                    </button>
                  </div>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted, computed } from 'vue'
import { useVuelidate } from '@vuelidate/core'
import Swal from 'sweetalert2'
import { required, email, maxLength, minLength } from '@vuelidate/validators'
import UserService from '@/services/UserService'

const loading = ref(false)
const message = reactive({
  show: false,
  type: 'success',
  text: ''
})

const form = reactive({
  hoTen: '',
  email: '',
  soDienThoai: '',
  diaChi: '',
  province: '',
  district: '',
  ward: '',
  specificAddress: ''
})

const provinces = reactive([])
const districts = reactive([])
const wards = reactive([])

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

const fullAddress = computed(() => {
  const parts = []
  if (form.specificAddress) {
    parts.push(form.specificAddress.replace(/,/g, ''))
  }
  const wardName = wards.find(w => w.code === form.ward)?.name_with_type
  if (wardName) parts.push(wardName)
  const districtName = districts.find(d => d.code === form.district)?.name_with_type
  if (districtName) parts.push(districtName)
  const provinceName = provinces.find(p => p.code === form.province)?.name_with_type
  if (provinceName) parts.push(provinceName)
  return parts.join(',')
})

const rules = {
  hoTen: { required, minLength: minLength(3), maxLength: maxLength(40) },
  email: { required, email, maxLength: maxLength(100) },
  soDienThoai: { maxLength: maxLength(20) },
  diaChi: { maxLength: maxLength(255) }
}

const v$ = useVuelidate(rules, form)
async function loadProfile() {
  try {
    const data = await UserService.getUserProfile()
    form.hoTen = data.hoTen ?? ''
    form.email = data.email ?? ''
    form.soDienThoai = data.soDienThoai ?? ''
    form.diaChi = data.diaChi ?? ''
    if (form.diaChi) {
      const parsed = parseAddress(form.diaChi)
      form.specificAddress = parsed.specificAddress
      if (parsed.province) {
        const provinceCode = findProvinceCode(parsed.province)
        if (provinceCode) {
          form.province = provinceCode
          await onProvinceChange(false)
          if (parsed.district) {
            const districtCode = findDistrictCode(parsed.district)
            if (districtCode) {
              form.district = districtCode
              await onDistrictChange(false)
              if (parsed.ward) {
                const wardCode = findWardCode(parsed.ward)
                if (wardCode) {
                  form.ward = wardCode
                }
              }
            }
          }
        }
      }
    }
  } catch (err) {
    Swal.fire({
      icon: 'error',
      title: 'Lỗi',
      text: err.message || 'Không thể tải thông tin người dùng'
    })
  }
}

async function handleSubmit() {
  const isValid = await v$.value.$validate()
  if (!isValid) return
  try {
    loading.value = true
    await UserService.updateProfile({
      HoTen: form.hoTen,
      Email: form.email,
      SoDienThoai: form.soDienThoai,
      DiaChi: fullAddress.value
    })
    form.diaChi = fullAddress.value
    Swal.fire({
      icon: 'success',
      title: 'Thành công',
      text: 'Cập nhật thông tin thành công',
      timer: 2000,
      showConfirmButton: false
    })
  } catch (err) {
    Swal.fire({
      icon: 'error',
      title: 'Thất bại',
      text: err.message || 'Cập nhật thông tin thất bại'
    })
  } finally {
    loading.value = false
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
    form.district = ''
    form.ward = ''
  }
  districts.length = 0
  wards.length = 0
  if (form.province) {
    try {
      const response = await fetch(`https://vn-public-apis.fpo.vn/districts/getByProvince?provinceCode=${form.province}&limit=-1`)
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
    form.ward = ''
  }
  wards.length = 0
  if (form.district) {
    try {
      const response = await fetch(`https://vn-public-apis.fpo.vn/wards/getByDistrict?districtCode=${form.district}&limit=-1`)
      const data = await response.json()
      if (data.exitcode === 1) {
        wards.push(...data.data.data)
      }
    } catch (error) {
      console.error('Error loading wards:', error)
    }
  }
}

onMounted(async () => {
  await loadProvinces()
  await loadProfile()
})
</script>

<style scoped>
.profile-wrapper {
  min-height: 60vh;
}

.account-wrapper {
  background: white;
  border-radius: 20px;
  box-shadow: 0 5px 30px rgba(0,0,0,0.08);
  overflow: hidden;
}

.account-content {
  padding: 2rem 2rem 1rem 2rem;
}

.account-section {
  margin-bottom: 2rem;
}

.section-titles {
  font-size: 1.25rem;
  font-weight: 600;
  color: var(--primary-color);
  margin-bottom: 1.5rem;
  padding-bottom: 1rem;
  border-bottom: 2px solid var(--light-color);
  display: flex;
  align-items: center;
}

.section-titles i {
  font-size: 1.4rem;
  margin-right: 0.5rem;
}

.account-form .form-group {
  margin-bottom: 1.5rem;
}

.account-form .form-label {
  font-weight: 500;
  color: var(--dark-color);
  margin-bottom: 0.5rem;
}

.account-form .input-group-text {
  background: var(--light-color);
  border: none;
  color: var(--primary-color);
}

.account-form .form-control {
  border: 2px solid var(--light-color);
  padding: 0.75rem 1rem;
  font-size: 0.95rem;
  transition: all 0.3s ease;
}

.account-form .form-control:focus {
  border-color: var(--primary-color);
  box-shadow: 0 0 0 0.2rem rgba(0,55,128,0.1);
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

.form-select {
  border: 2px solid var(--light-color);
  padding: 0.75rem 1rem;
  font-size: 0.95rem;
  transition: all 0.3s ease;
}

.form-select:focus {
  border-color: var(--primary-color);
  box-shadow: 0 0 0 0.2rem rgba(0,55,128,0.1);
}

.btn-update {
  padding: 0.75rem 2rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  background: var(--gradient);
  border: none;
  transition: all 0.3s ease;
}

.btn-update:hover {
  transform: translateY(-2px);
  box-shadow: 0 5px 15px rgba(0,55,128,0.2);
}
</style>
