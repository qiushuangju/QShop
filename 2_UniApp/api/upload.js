import request from '@/utils/request'

// api地址
const api = {
  image: 'FileUpload/uploadImg'
}

// 图片上传
export const uploadImage = files => {
  // 文件上传大小, 2M
  const maxSize = 1024 * 1024 * 2
  // 执行上传
  return new Promise((resolve, reject) => {
    request.urlFileUpload({ files, maxSize }).then(arrRes => 	{
		console.log("shangchuan",JSON.stringify(arrRes))
		var listData=[];
		arrRes.forEach(item => {
			item.result.forEach(data=>{
				listData.push(data)
			})
        })
		
		console.log("listFile",JSON.stringify(listData))
	  resolve(listData.map(item => item.id), listData)
	  })
      .catch(reject)
  })
}
