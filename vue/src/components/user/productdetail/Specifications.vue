<template>
  <div class="tab-pane fade" id="specifications" role="tabpanel">
    <div class="product-specifications">
      <!-- Có thể thêm table-striped tùy vào phong cách -->
      <table class="table">
        <tbody>
          <tr>
            <th>Màu sắc: </th>
            <td>
              <span v-if="product.mauSac">{{ product.mauSac }}</span>
              <span v-else class="not-updated">Chưa cập nhật</span>
            </td>
          </tr>
          <tr>
            <th>Kích thước: </th>
            <td>
              <span v-if="product.kichThuoc">{{ product.kichThuoc }}</span>
              <span v-else class="not-updated">Chưa cập nhật</span>
            </td>
          </tr>
          <tr>
            <th>Chất liệu: </th>
            <td>
              <span v-if="product.chatLieu">{{ product.chatLieu }}</span>
              <span v-else class="not-updated">Chưa cập nhật</span>
            </td>
          </tr>
          <tr>
            <th>Xuất xứ: </th>
            <td>
              <span v-if="product.xuatXu">{{ product.xuatXu }}</span>
              <span v-else class="not-updated">Chưa cập nhật</span>
            </td>
          </tr>
          <tr>
            <th>Bảo hành: </th>
            <td>
              <span v-if="product.baoHanh">{{ product.baoHanh }}</span>
              <span v-else class="not-updated">Chưa cập nhật</span>
            </td>
          </tr>
        </tbody>
      </table>
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
.table tbody tr {
  transition: all 0.3s ease;
}

.table tbody tr:hover {
  background-color: rgba(0,0,0,0.01);
}

.table th,
.table td {
  padding: 1.2rem 1.5rem;
  border-color: rgb(230 230 230);
  vertical-align: middle;
}

.table th {
  width: 30%;
  font-weight: 600;
  border-right: 1px solid rgb(230 230 230);
}

.not-updated {
  color: #856404;
  background-color: #fff3cd;
  padding: 2px 6px;
  border-radius: 6px;
  font-weight: 500;
}

@media (max-width: 1200px) {
  .table th,
  .table td {
    padding: 1.1rem 1.3rem;
    font-size: 0.95rem;
  }
}

@media (max-width: 992px) {
  .table th,
  .table td {
    padding: 1rem 1.2rem;
    font-size: 0.93rem;
  }
  
  .table th {
    width: 35%;
  }
}

@media (max-width: 768px) {
  .product-specifications {
    margin: 0.5rem auto;
  }
  
  .table th,
  .table td {
    padding: 1rem;
    font-size: 0.9rem;
  }
  
  .table th {
    width: 40%;
  }
  
  .not-updated {
    padding: 2px 5px;
    font-size: 0.88rem;
  }
}

@media (max-width: 576px) {
  .product-specifications {
    overflow-x: auto;
    -webkit-overflow-scrolling: touch;
  }
  
  .table {
    border-radius: 8px;
    font-size: 0.85rem;
    min-width: 100%;
  }
  
  .table th,
  .table td {
    padding: 0.75rem;
    font-size: 0.85rem;
  }
  
  .table th {
    width: 45%;
    font-size: 0.83rem;
  }
  
  .not-updated {
    padding: 2px 4px;
    font-size: 0.8rem;
    border-radius: 4px;
  }
}

@media (max-width: 400px) {
  .table th,
  .table td {
    padding: 0.65rem 0.5rem;
    font-size: 0.8rem;
  }
  
  .table th {
    width: 50%;
  }
  
  .not-updated {
    font-size: 0.75rem;
    padding: 1px 3px;
  }
}

@media print {
  .product-specifications {
    box-shadow: none;
  }
  
  .table th, 
  .table td {
    border: 1px solid #dee2e6;
    padding: 0.8rem;
  }
}
</style>