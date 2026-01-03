<template>
  <div class="tab-pane fade show active" id="description" role="tabpanel">
    <div class="detail-description">
      <div v-if="product.moTaChiTiet" v-html="product.moTaChiTiet"></div>
      <div v-else class="no-description">
        Hiện tại chưa có mô tả chi tiết cho sản phẩm này. Chúng tôi sẽ cập nhật sớm nhất !!
      </div>
    </div>
  </div>
</template>

<script setup>
defineProps({
  product: {
    type: Object,
    required: true
  }
})
</script>

<style scoped>
.detail-description {
  color: #444;
  line-height: 1.8;
  font-size: 1rem;
}

.no-description {
  padding: 1rem;
  background: #fff3cd;
  border-left: 5px solid #ffeeba;
  border-radius: 8px;
  color: #856404;
  font-weight: 500;
  text-align: center;
  font-size: 0.95rem;
}

@media (max-width: 1200px) {
  .detail-description {
    font-size: 0.95rem;
    line-height: 1.7;
  }
  
  .no-description {
    font-size: 0.9rem;
  }
}

@media (max-width: 992px) {
  .detail-description {
    font-size: 0.93rem;
    line-height: 1.7;
  }
  
  .no-description {
    padding: 0.9rem;
    font-size: 0.88rem;
  }
}

@media (max-width: 768px) {
  .detail-description {
    font-size: 0.9rem;
    line-height: 1.6;
  }
  
  .no-description {
    padding: 0.8rem;
    font-size: 0.85rem;
    border-left-width: 4px;
  }
}

@media (max-width: 576px) {
  .detail-description {
    font-size: 0.88rem;
    line-height: 1.6;
  }
  
  .no-description {
    padding: 0.75rem;
    font-size: 0.83rem;
    border-radius: 6px;
  }
}

@media (max-width: 400px) {
  .detail-description {
    font-size: 0.85rem;
    line-height: 1.5;
  }
  
  .no-description {
    padding: 0.7rem;
    font-size: 0.8rem;
    border-left-width: 3px;
  }
}
</style>