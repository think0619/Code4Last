<template>
    <div class="content">
        <!-- <van-nav-bar class="vannav" :border=false :title="contentTitle" left-text="" left-arrow @click-left="onClickLeft" /> -->
        <div class="contentTitle">
            <span>{{ contentTitle }}</span>
        </div>
        <div class="btnArea">
            <van-space direction="vertical" fill size="2vh">
                <div id="page1_btn1" class="cmdCustomBtn1 cmdbtn1-1" @click="sendmsg('page1_btn1', 'video_pv01')">
                    <span class="btn_name">功能按键-1</span>
                    <van-image round class="crtIcon" fit="cover" :src="correcticon" v-show="showlist.page1_btn1" />
                </div>
                <div id="page1_btn2" class="cmdCustomBtn1 cmdbtn1-2" @click="sendmsg('page1_btn2', 'video_pv02')">
                    <span class="btn_name">功能按键-2</span>
                    <van-image round class="crtIcon" fit="cover" :src="correcticon" v-show="showlist.page1_btn2" />
                </div>
                <div id="page1_btn3" class="cmdCustomBtn1 cmdbtn1-3" @click="sendmsg('page1_btn3', 'video_pv03')">
                    <span class="btn_name">功能按键-3</span>
                    <van-image round class="crtIcon" fit="cover" :src="correcticon" v-show="showlist.page1_btn3" />
                </div>
                <div id="page1_btn4" class="cmdCustomBtn1 cmdbtn1-4" @click="sendmsg('page1_btn4', 'video_pv04')">
                    <span class="btn_name">功能按键-4</span>
                    <van-image round class="crtIcon" fit="cover" :src="correcticon" v-show="showlist.page1_btn4" />
                </div>
                <div id="page1_btn1" class="cmdCustomBtn1 cmdbtn2-1" @click="sendmsg('page1_btn5', 'video_pv05')">
                    <span class="btn_name">功能按键-5</span>
                    <van-image round class="crtIcon" fit="cover" :src="correcticon" v-show="showlist.page1_btn5" />
                </div>
                <div id="page1_btn2" class="cmdCustomBtn1 cmdbtn2-2" @click="sendmsg('page1_btn6', 'video_pv06')">
                    <span class="btn_name">功能按键-6</span>
                    <van-image round class="crtIcon" fit="cover" :src="correcticon" v-show="showlist.page1_btn6" />
                </div>
                <div id="page1_btn3" class="cmdCustomBtn1 cmdbtn2-3" @click="sendmsg('page1_btn7', 'video_pv07')">
                    <span class="btn_name">功能按键-7</span>
                    <van-image round class="crtIcon" fit="cover" :src="correcticon" v-show="showlist.page1_btn7" />
                </div>
                <div id="page1_btn4" class="cmdCustomBtn1 cmdbtn2-4" @click="sendmsg('page1_btn8', 'video_pv08')">
                    <span class="btn_name">功能按键-8</span>
                    <van-image round class="crtIcon" fit="cover" :src="correcticon" v-show="showlist.page1_btn8" />
                </div>

            </van-space>
            <!-- <van-image round class="cmdResetBtn" fit="cover" @click="onclickReset('page1_reset')" :src="customicons.reset" /> -->

            <!-- <div class="arrowdiv">
                <van-row justify="space-between" align="center" gutter="0">
                    <van-col span="8" class="arrowCol"> <van-image round class=" " fit="cover" @click="changeBtn('pre')"
                            :src="leftarrow" /></van-col>
                    <van-col span="8" class="arrowCol"></van-col>
                    <van-col span="8" class="arrowCol"> <van-image round class=" " fit="cover" @click="changeBtn('next')"
                            :src="rightarrow" /></van-col>
                </van-row>
            </div> -->


        </div>
        <van-action-bar>
            <van-action-bar-icon :icon="customicons.replay" text="重置" @click="clickcontrol('reset')" />
            <van-action-bar-icon :icon="customicons.play" text="开始" @click="clickcontrol('start')" />
            <van-action-bar-icon :icon="customicons.stop" text="暂停" @click="clickcontrol('pause')" />
            <van-action-bar-icon :icon="customicons.rewind" text="上一个" @click="clickcontrol('previous')" />
            <van-action-bar-button type="primary" text="下一个" @click="clickcontrol('next')" />
        </van-action-bar>




        <!-- <van-tabbar v-model="active" @change="clickcontrol"> 
                <van-tabbar-item name="reset">
                    <span>重置</span>
                    <template #icon="props">
                        <img :src="customicons.replay" />
                    </template>
                </van-tabbar-item>

               <van-tabbar-item name="start" >
                    <span>开始</span>
                    <template #icon="props">
                        <img :src="customicons.play" />
                    </template>
                </van-tabbar-item>

                <van-tabbar-item name="pause">
                    <span>暂停</span>
                    <template #icon="props">
                        <img :src="customicons.stop" />
                    </template>
                </van-tabbar-item>
              
                <van-tabbar-item name="previous">
                    <span>上一个</span>
                    <template #icon="props">
                        <img :src="customicons.rewind" />
                    </template>
                </van-tabbar-item>

                <van-tabbar-item name="next">
                    <span>下一个</span>
                    <template #icon="props">
                        <img :src="customicons.forward"  />
                    </template>
                </van-tabbar-item> 
            </van-tabbar> -->
    </div>
</template>   

<script setup lang="jsx">
import { showSuccessToast, showFailToast, showToast } from 'vant';
import correcticon from '@/assets/img/icons8-checkmark.svg';


import { login } from "@/api/config";
import { getMQTTInfo } from "@/api/mqtthelper";
import * as mqtt from "mqtt/dist/mqtt.min";
</script> 

<script lang="jsx">
import mediaPlayIcon from '@/assets/img/media/icons8-play-64.png';
import mediaForwardIcon from '@/assets/img/media/icons8-fast-forward-64.png';
import mediaRewindIcon from '@/assets/img/media/icons8-rewind-64.png';
import mediaStopIcon from '@/assets/img/media/icons8-stop-64.png';
import mediaReplayIcon from '@/assets/img/media/icons8-replay-64.png';
import resetBtnImg from '@/assets/img/resetbtn.png';

export default {
    components: {
    },
    data() {
        return {
            contentTitle: 'CCTEG · 国之担当',
            active: 0,
            show: false,
            curBtnIndex: -1,
            // scrolltop:0,
            pageBtnArrayKey: ['page1_btn1', 'page1_btn2', 'page1_btn3', 'page1_btn4',],
            showlist: {
                page1_btn1: false,
                page1_btn2: false,
                page1_btn3: false,
                page1_btn4: false,
                page1_btn5: false,
                page1_btn6: false,
                page1_btn7: false,
                page1_btn8: false,
            },
            customicons: {
                forward: mediaForwardIcon,
                rewind: mediaRewindIcon,
                stop: mediaStopIcon,
                play: mediaPlayIcon,
                reset: resetBtnImg,
                replay: mediaReplayIcon
            }

        };
    },
    computed: {
    },
    setup() {

    },
    mounted() {
        //  this.$emit("onChangeTitle", this.title);
        const idcode = this.$route.query.idcode;
        if (idcode != null) {
            login(idcode).then((reqresult) => {
                if (reqresult.data) {
                    let result = reqresult.data; //Msg Token 
                    if (result.Status == 1) {
                        var token = result.Token;
                        //get mqtt info 
                        if (token != null) {
                            showSuccessToast({
                                "wordBreak": "break-word",
                                "message": "Authenticated.",
                                "duration": 800
                            });
                            getMQTTInfo(token).then((mqttr) => {
                                if (mqttr != null && mqttr.Status == 1) {
                                    let mqttdata = mqttr.Data;
                                    let mqtt_wsurl = mqttdata.wsurl;
                                    let mqtt_username = mqttdata.username;
                                    let mqtt_password = mqttdata.password;
                                    this.connectMQTT(mqtt_wsurl, mqtt_username, mqtt_password)
                                } else {
                                    showFailToast({
                                        "wordBreak": "break-word",
                                        "message": "认证过期，刷新页面.",
                                        "duration": 800
                                    });
                                }
                            });
                        }
                    } else {
                        showFailToast({
                            "wordBreak": "break-word",
                            "message": "认证失败." + result.Msg,
                            "duration": 800
                        });
                    }
                } else {
                    showFailToast('login error')
                }
            })
        } else {
            showFailToast('The url is error.')
        }

    },
    methods: {
        connectMQTT(_url, _username, _password) {
            let that = this;

            let mqttid = that.$store.state.mqttid;
            if (!mqttid) {
                let _mqttid = 'ExibitionMobile_' + Math.random().toString(16).substr(2, 8);
                that.$store.commit('setToken', _mqttid)
                console.log(that.$store.state.mqttid)
            }

            that.mqttclient = mqtt.connect(_url, {
                clientId: that.$store.state.mqttid,
                keepalive: 60,
                reconnectPeriod: 30000,
                username: _username,
                password: _password
            })
            that.mqttclient.on('connect', function () {
                // showSuccessToast({
                //     "wordBreak": "break-word",
                //     "message": "The mqtt client has connected.",
                //     "duration": 800
                // });
                // that.mqttclient.subscribe('ShowClockTime', function (err) {
                //     if (!err) { } else { }
                // }) 
            })
            that.mqttclient.on('message', function (topic, message) {
                // console.log(topic.toString())
                // console.log(message.toString())
            })
        },

        publicmqtt(msg) {
            //cmd$Beijing${GUID}_restartapp 
            //cmd$Beijing${GUID}_video_pv01
            //cmd$Beijing${GUID}_video_pause
            //cmd$Beijing${GUID}_video_start
            //cmd$Beijing${GUID}_video_single
            //cmd$Beijing${GUID}_video_list 
            //cmd$Beijing${GUID}_video_next 
            //cmd$Beijing${GUID}_video_previous

            if (msg.length > 0) {
                const topic = "ExhibitionPC"
                const GUID = 'YUIDX43';
                var fullmsg = "cmd$Beijing$" + GUID + "_" + msg;
                let that = this;
                if (that.mqttclient != null) {
                    that.mqttclient.publish(topic, fullmsg, {
                        qos: 0,
                        retain: false
                    }, function (err) {
                        if (err) {
                            showToast({
                                "wordBreak": "break-word",
                                "message": "发送失败，" + err,
                                "duration": 800,
                                'icon': 'fail'
                            })
                        } else {
                            showToast({
                                "wordBreak": "break-word",
                                "message": "发送成功",
                                "duration": 800,
                                'icon': 'success'
                            })
                        }
                    })
                } else {
                    showToast({
                        "wordBreak": "break-word",
                        "message": "The mqtt client has disconnected.",
                        "duration": 800,
                        'icon': 'fail'
                    })
                }
            }
        },

        sendmsg(btnkey, msg) {
            let that = this;
            // for (let key in that.showlist) {
            //     if (key == btnkey) {
            //         that.showlist[key] = true;
            //     } else {
            //         that.showlist[key] = false;
            //     }
            // }
            this.publicmqtt(msg);
        },

        clickcontrol(name) {
            let msg = "";
            switch (name) {
                case "reset": msg = "video_pv01"; break;
                case "start": msg = "video_start"; break;
                case "pause": msg = "video_pause"; break;
                case "previous": msg = "video_previous"; break;
                case "next": msg = "video_next"; break;
            }

            // if (name === "reset") {
            //     let that = this;
            //     for (let key in that.showlist) {
            //     if (key == "page1_btn1") {
            //         that.showlist[key] = true;
            //     }else{
            //         that.showlist[key] = false;
            //     }
            // } 

       // }
            if(msg) {
            this.publicmqtt(msg);
        }

    }
}
}; 
</script>



<style>
:root:root {
    --van-nav-bar-title-text-color: #fff;
    --van-nav-bar-icon-color: #fff;
    --van-nav-bar-arrow-size: 20px;

    --van-tabbar-height: 60px;
    --van-tabbar-background: rgba(13, 37, 111, 0.800);

    --van-tabbar-item-active-background: transparent;
    --van-tabbar-item-icon-size: 24px;
    --van-tabbar-item-font-size: 16px;
    --van-tabbar-item-text-color: #FFF;
    --van-tabbar-item-active-color: #d1ccccbd;

    --van-action-bar-background: rgba(13, 37, 111, 0.800);
    --van-action-bar-icon-background: rgba(13, 37, 111, 0.800);
    --van-action-bar-height: 60px;
    --van-action-bar-icon-width: 55px;
    --van-action-bar-icon-height: 100%;
    --van-action-bar-icon-size: 24px;
    --van-action-bar-icon-font-size: 15px;
    --van-action-bar-icon-text-color: #FFF;

}

.van-nav-bar {
    /* background: url(../../assets/img/navbarbg.png); */
    background-color: rgba(13, 37, 111, 0.800);
    background-repeat: no-repeat;
    background-size: 100vw 100vh;
    --van-nav-bar-height: 6vh
}

.content {
    height: 100%;
    background: url(@/assets/img/back/itembg.png);
    background-repeat: no-repeat;
    background-size: 100vw 100vh;
}

.contentTitle {
    width: 100vw;
    margin: 0 auto;
    height: 50px;
    color: #fffefc;
    margin: 0vh auto 0 auto;
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: center;
    font-size: 1.5rem;
    font-weight: bold;
    /* font-family: Youshe; */
}

.btnArea {
    /* max-height: 70vh; */
    max-height: calc(92vh - var(--van-action-bar-height) - 50px);
    overflow: scroll;
    margin: 10px auto;
}

.cmdCustomBtn1 {
    margin: 0 auto;
    width: 90vw;
    height: 12vh;
    --van-button-border-width: 0;
    display: flex;
    -webkit-tap-highlight-color: rgba(0, 0, 0, 0);

}

.cmdCustomBtn1 .btn_name {
    line-height: 12vh;
    font-size: 1.5rem;
    font-weight: 600;
    color: #fff;
    margin-left: 30px;
}

.cmdCustomBtn2 {
    width: 90vw;
    line-height: 50px;
    font-size: 1.5rem;
    font-weight: 600;
    color: #fff;
    margin: 10px auto;
}

.cmdResetBtn {
    position: absolute;
    top: 1vh;
    right: 5vw;
    width: 3rem;
    height: 3rem;
}

.arrowdiv {
    position: absolute;
    bottom: 18vh;
    width: 100vw;
    height: 6vh;
    margin: 0 auto;

}

.arrowCol {
    text-align: center;
}

.crtIcon {
    margin-left: auto;
    margin-top: 2vh;
    margin-right: 2vw;
    width: 2rem;
    height: 2rem;
}

.custabbar {
    --van-tabbar-height: 7.5vh
}

.cmdbtn-size {
    background-repeat: no-repeat;
    background-size: cover;
}

.cmdbtn1 {
    background: url(@/assets/img/btn/01-1.jpg);
    background-repeat: no-repeat;
    background-size: cover;
}

.cmdbtn1-1 {
    background: url(@/assets/img/btn/01-1.jpg);
    background-repeat: no-repeat;
    background-size: cover;
}

.cmdbtn1-2 {
    background: url(@/assets/img/btn/01-2.jpg);
    background-repeat: no-repeat;
    background-size: cover;
}

.cmdbtn1-3 {
    background: url(@/assets/img/btn/01-3.jpg);
    background-repeat: no-repeat;
    background-size: cover;
}

.cmdbtn1-4 {
    background: url(@/assets/img/btn/01-4.jpg);
    background-repeat: no-repeat;
    background-size: cover;
}

.cmdbtn2-1 {
    background: url(@/assets/img/btn/02-1.jpg);
    background-repeat: no-repeat;
    background-size: cover;
}

.cmdbtn2-2 {
    background: url(@/assets/img/btn/02-2.jpg);
    background-repeat: no-repeat;
    background-size: cover;
}

.cmdbtn2-3 {
    background: url(@/assets/img/btn/02-3.jpg);
    background-repeat: no-repeat;
    background-size: cover;
}

.cmdbtn2-4 {
    background: url(@/assets/img/btn/02-4.jpg);
    background-repeat: no-repeat;
    background-size: cover;
}

.cmdbtn3-1 {
    background: url(@/assets/img/btn/03-1.jpg);
    background-repeat: no-repeat;
    background-size: cover;
}

.cmdbtn3-2 {
    background: url(@/assets/img/btn/03-2.jpg);
    background-repeat: no-repeat;
    background-size: cover;
}

.cmdbtn3-3 {
    background: url(@/assets/img/btn/03-3.jpg);
    background-repeat: no-repeat;
    background-size: cover;
}

.cmdbtn3-4 {
    background: url(@/assets/img/btn/03-4.jpg);
    background-repeat: no-repeat;
    background-size: cover;
}
</style>
