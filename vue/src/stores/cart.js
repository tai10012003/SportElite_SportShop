import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export const useCartStore = defineStore('cart', () => {
    const items = ref({})
    
    const subtotal = computed(() => {
        return Object.values(items.value).reduce((sum, item) => sum + item.price * item.quantity, 0)
    })
    
    const shippingFee = computed(() => (subtotal.value >= 500000 ? 0 : 30000))
    const total = computed(() => subtotal.value + shippingFee.value)
    const itemCount = computed(() => Object.keys(items.value).length)
    
    const addItem = (product, selectedColor = '', selectedSize = '', quantity = 1) => {
        const itemKey = `${product.id}_${selectedColor}_${selectedSize}`
        if (items.value[itemKey]) {
            items.value[itemKey].quantity += quantity
        } else {
            items.value[itemKey] = {
                id: product.id,
                itemKey: itemKey,
                name: product.tenSanPham,
                price: product.giaKhuyenMai || product.gia,
                image: product.mainImage || (product.hinhAnh?.[0]?.duongDan || '/images/no-image.jpg'),
                slug: product.slug,
                color: selectedColor,
                size: selectedSize,
                quantity: quantity
            }
        }
    }
    
    const updateQuantity = (itemKey, quantity) => {
        if (quantity < 1) {
            removeItem(itemKey)
            return
        }
        if (items.value[itemKey]) {
            items.value[itemKey].quantity = quantity
        }
    }
    
    const removeItem = (itemKey) => {
        delete items.value[itemKey]
    }
    
    const clearCart = () => {
        items.value = {}
    }
    
    return {
        items,
        subtotal,
        shippingFee,
        total,
        itemCount,
        addItem,
        updateQuantity,
        removeItem,
        clearCart
    }
})