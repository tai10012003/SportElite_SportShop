import { defineStore } from 'pinia'
import { ref, computed, watch } from 'vue'

export const useCartStore = defineStore('cart', () => {
    const items = ref(JSON.parse(localStorage.getItem('cart_items')) || {})

    const subtotal = computed(() =>Object.values(items.value).reduce((sum, item) => sum + item.price * item.quantity,0))
    const shippingFee = computed(() =>subtotal.value >= 500000 ? 0 : 30000)
    const total = computed(() => subtotal.value + shippingFee.value)
    const itemCount = computed(() => Object.keys(items.value).length)

    const addItem = (product, selectedColor = '', selectedSize = '', quantity = 1) => {
        const itemKey = `${product.maSanPham}_${selectedColor}_${selectedSize}`
        if (items.value[itemKey]) {
            items.value[itemKey].quantity += quantity
        } else {
            items.value[itemKey] = {
                id: product.id,
                maSanPham: product.maSanPham,
                itemKey,
                name: product.tenSanPham,
                price: product.giaKhuyenMai || product.gia,
                image: product.mainImage || product.hinhAnh?.[0]?.duongDan || '/images/no-image.jpg',
                slug: product.slug,
                color: selectedColor,
                size: selectedSize,
                quantity
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

    watch(
        items,
        (newItems) => {
            localStorage.setItem('cart_items', JSON.stringify(newItems))
        },
        { deep: true }
    )
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