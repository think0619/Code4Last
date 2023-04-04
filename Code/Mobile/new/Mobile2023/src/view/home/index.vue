<template>
    <div id="content">  
        <div class="btn1"> 
            <van-button type="primary"
            size="normal"
            block 
            @click="goto('/main/t1')">主要按钮</van-button>
        </div>
        <!-- <div @click="goto('/main/t1')">跳转绿色能源</div> -->
    </div>  
</template>

<script setup> 
import { useRouter, useRoute } from 'vue-router'
const router = useRouter()
// const route = useRoute()
// console.log('路由', route.path)
const goto = (path) => {
    router.replace(path)
}
</script>
 


<style  >
#content{ 
    height: 100%;
    margin: 0;
    padding: 0;
    overflow: hidden;
    background: url(../../assets/img/index1.jpg);
    background-size: cover;
}
.btn1{
    position:absolute;
    left:20px;
    bottom:220px;
    width: 120px;;
}

.cusbtn{
    margin: 10px;;
    width: 200px;;
 }




</style>