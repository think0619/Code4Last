import Vue from 'vue';
import Router from 'vue-router';

Vue.use(Router);

const routes = [
    // {
    //     path: '*',
    //     redirect: '/goods'
    // },
    {
        name: 'exhi',
        component: () =>
            import ('./view/exhibition'),
        meta: {
            title: '会员中心'
        }
    }, {
        name: '',
        component: () =>
            import ('./view/exhibition/main'),
        meta: {
            title: '展区中控'
        }
    },
    {
        name: 'cart',
        component: () =>
            import ('./view/cart/'),
        meta: {
            title: '模型测试'
        }
    },

    {
        name: 'user',
        component: () =>
            import ('./view/user'),
        meta: {
            title: '会员中心'
        }
    },
    {
        name: 'card',
        component: () =>
            import ('./view/card'),
        meta: {
            title: '购物车'
        }
    },
    // {
    //     name: 'goods',
    //     component: () =>
    //         import ('./view/goods'),
    //     meta: {
    //         title: '商品详情'
    //     }
    // }
];

// add route path
routes.forEach(route => {
    route.path = route.path || '/' + (route.name || '');
});

const router = new Router({ routes });

router.beforeEach((to, from, next) => {
    const title = to.meta && to.meta.title;
    if (title) {
        document.title = title;
    }
    next();
});

export {
    router
};