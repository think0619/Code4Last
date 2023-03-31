<template>
  <!-- <img alt="Vue logo" src="./assets/logo.png">-->
  <TestName  /> 
  <!-- <div id="app">
    <router-view />
  </div> -->
</template>

<script>
import TestName from './components/Index/TestName.vue'

export default {
  name: 'App',
  components: {
    TestName
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin:  0px 0 0px 0;
  bottom: 100px;
}

</style>
