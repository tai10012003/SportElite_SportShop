export const colorMap = {
  // Màu cơ bản
  'đen': '#000000',
  'trắng': '#FFFFFF',
  'xám': '#808080',
  'bạc': '#C0C0C0',

  // Màu đỏ và họ đỏ
  'đỏ': '#FF0000',
  'đỏ tươi': '#FF3333',
  'đỏ đậm': '#CC0000',
  'đỏ thẫm': '#8B0000',
  'hồng': '#FFC0CB',
  'cam': '#FFA500',

  // Màu xanh lá và họ xanh lá
  'xanh lá': '#00FF00', 
  'xanh lá đậm': '#008000',
  'xanh lá nhạt': '#90EE90',
  'xanh rêu': '#556B2F',
  'xanh olive': '#808000',

  // Màu xanh dương và họ xanh dương
  'xanh dương': '#0000FF',
  'xanh biển': '#000080',
  'xanh da trời': '#87CEEB',
  'xanh coban': '#4169E1',
  'xanh ngọc': '#40E0D0',
  'xanh navy': '#000080',

  // Màu vàng và họ vàng 
  'vàng': '#FFFF00',
  'vàng nhạt': '#FFFFE0',
  'vàng đậm': '#FFD700',
  'be': '#F5F5DC',

  // Màu tím và họ tím
  'tím': '#800080',
  'tím nhạt': '#E6E6FA',
  'tím đậm': '#4B0082',

  // Màu nâu và họ nâu
  'nâu': '#8B4513',
  'nâu đất': '#A0522D',
  'nâu nhạt': '#DEB887',
  'kem': '#FFF8DC',

  // Màu thể thao phổ biến
  'neon xanh': '#00FF7F',
  'neon vàng': '#FFFF00',
  'neon hồng': '#FF69B4',
  'phản quang': '#F0F0F0',
  'gradient xanh': 'linear-gradient(45deg, #00ff00, #008000)',
  'gradient đỏ': 'linear-gradient(45deg, #ff0000, #8b0000)'
}

const normalizeColorName = (color) => {
  if (!color) return ''
  return color.trim().replace(/Ð/g, 'Đ').replace(/ð/g, 'đ').toLowerCase().replace(/\s+/g, ' ')
}

export const getColorStyle = (color) => {
  const normalizedColor = normalizeColorName(color)
  const colorValue = colorMap[normalizedColor]
  const finalColor = colorValue || '#CCCCCC'
  if (!colorValue && color) {
    console.warn(`⚠️ Màu "${color}" (normalized: "${normalizedColor}") không tồn tại trong colorMap`)
  }
  return {
    backgroundColor: finalColor,
    borderColor: normalizedColor === 'trắng' ? '#ddd' : finalColor
  }
}

export const isValidColor = (color) => {
  const normalizedColor = normalizeColorName(color)
  return colorMap.hasOwnProperty(normalizedColor)
}

export const getAvailableColors = () => {
  return Object.keys(colorMap)
}