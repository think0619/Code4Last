<template>
  <div class="content">
    <van-nav-bar class="vannav" :border=false :title="str1" left-text="" left-arrow @click-left="onClickLeft" />
    <router-view ref="child" @onChangeTitle="changeTitle" @onDoCMDSend="doCMDSend" />
  </div>
  <div>
    <van-tabbar v-model="active" @change="onChange" :border="false" safe-area-inset-bottom="true" active-color="#333"
      inactive-color="#FFF">
      <van-tabbar-item icon="home-o" name="/main/t1" to="/main/t1" replace="true" badge="5">智能喷涂</van-tabbar-item>
      <van-tabbar-item icon="search" name="/main/t2" to="/main/t2" replace="true" dot>智能井口</van-tabbar-item>
      <van-tabbar-item icon="friends-o" name="/main/t3" to="/main/t3" replace="true"  badge="5">绿色低碳园区</van-tabbar-item>
      <van-tabbar-item icon="setting-o" name="/main/t4" to="/main/t4" replace="true"  dot>生态治理</van-tabbar-item>
    </van-tabbar>
  </div>
</template> 

<script setup>
import { ref } from 'vue';
 

import { useRouter, useRoute } from 'vue-router';
const router = useRouter();
const route = useRoute();
 
const active = ref(route.path);
</script >

<script>
import { sendCMDMsg } from "@/api/index";
import { showToast } from 'vant';

// cmd$PC${GUID}_video_pv01
// cmd$PC${GUID}_video_pause
// cmd$PC${GUID}_video_start
// cmd$PC${GUID}_video_single
// cmd$PC${GUID}_video_list 
// cmd$PC${GUID}_video_next 
// cmd$PC${GUID}_video_previous
// cmd$Model$m1_open01 cmd$Model${GUID}_open01  

export default {
  components: {
  },
  data() {
    return {
      str1: '1',
      active: 0,
      cmds:
        [
          { key: 'page1_btn1', cmd: ["cmd$Model$m1_open01", "cmd$PC$V4DES_Video_PV01",] },
          { key: 'page1_btn2', cmd: ["cmd$Model$m1_open02", "cmd$PC$V4DES_Video_PV02", ] },
          { key: 'page1_btn3', cmd: ["cmd$Model$m1_open03", "cmd$PC$V4DES_Video_PV03", ] },
          { key: 'page1_btn4', cmd: ["cmd$Model$m1_open04", "cmd$PC$V4DES_Video_PV04", ] },
          
          { key: 'page2_btn1', cmd: ["cmd$Model$m1_open04", "cmd$PC$V4DES_Video_pause", ] },
          { key: 'page2_btn2', cmd: ["cmd$Model$m1_open04", "cmd$PC$V4DES_Video_start", ] },
          { key: 'page2_btn3', cmd: ["cmd$Model$m1_open04", "cmd$PC$V4DES_Video_single", ] },
          { key: 'page2_btn4', cmd: ["cmd$Model$m1_open04", "cmd$PC$V4DES_Video_list ",] },
        ]
    };
  },
  computed: {
  },
  setup() {
  },
  mounted() {

  },
  methods: {
    onClickLeft() {
      this.$router.replace('/home')
    },
    changeTitle(_title) {
      this.str1 = _title;
    },
    doCMDSend(btnKey) {
            let that = this;
            var sendContent = "";
            that.cmds.forEach(function (value, index, array) {
                if (btnKey == value.key) {
                    sendContent = value.cmd;
                }
            });
            if(sendContent.length>0){
            sendCMDMsg({
                Msg: sendContent,
                Sender: 'zhkjdev',
            }).then((res) => { 
                console.log(res);
                if (res != null && res.data != null) {
                    console.log(res.data);
                    showToast('提示内容');
                    var qresult = res.data;
                    if (qresult != null) {
                        //success
                        if (qresult.Status == 1) {
                            showToast({
                                position: 'top', 
                                message:'发送成功',
                                duration:500,
                            }); 
                        } else {
                            showToast({
                                position: 'top', 
                                message:'发送失败',
                                duration:500,
                            });  
                        }
                    } else {
                        that.errorToast();
                    }
                } else {
                    that.errorToast();
                }
            }).catch(function (err) { 
                    console.log(err);
                    that.errorToast();
                });  
            }
            else {
                showToast('系统错误');
            }
        },
        errorToast() {
            showToast({
                position: 'top',
                message: '发送异常',
                duration: 500,
            });
        },
  }
}; 
</script>

<style>
/*底部导航*/
.body-footer {
  display: none;
  position: fixed;
  bottom: 0;
  height: 1.69rem;
  width: 100%;
  color: #999999;
  font-size: 0.24rem;
  line-height: 0.51rem;
}

.content {
  background: url(../../assets/img/back.png);
  background-size: cover;
  height: 100%;
}
</style>

<style>
:root:root {
  --van-nav-bar-title-text-color: #fff;
  --van-nav-bar-icon-color: #fff;
  --van-nav-bar-arrow-size: 20px;

  --van-tabbar-background: transparent;
  --van-tabbar-item-icon-size: 22px;
}

.van-nav-bar {
  background: url(../../assets/img/navbarbg.png) !important;
  background-size: cover;
}
</style>
