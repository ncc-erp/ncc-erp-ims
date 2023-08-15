declare global {
    interface RouterMeta {
        title: string;
    }
    interface Router {
        hidden?: boolean;
        path: string;
        name: string;
        icon?: string;
        permission?: string;
        meta?: RouterMeta;
        component: any;
        children?: Array<Router>;
    }
    interface System {
        import(request: string): Promise<any>
    }
    var System: System
}
import login from '../views/login.vue'
import home from '../views/home/home.vue'
import main from '../views/main.vue'

export const locking = {
    path: '/locking',
    name: 'locking',
    component: () => import('../components/lockscreen/components/locking-page.vue')
};
export const loginRouter: Router = {
    path: '/',
    name: 'login',
    meta: {
        title: 'LogIn'
    },
    component: () => import('../views/login.vue')
};
export const otherRouters: Router = {
    path: '/main',
    name: 'main',
    permission: '',
    meta: { title: 'Information' },
    component: main,
    children: [
        { path: 'home', meta: { title: 'HomePage' }, name: 'home', component: () => import('../views/dashboard/dashboard.vue') },
    ]
}
export const appRouters: Array<Router> = [
    //     {
    //     path: '/setting',
    //     name: 'setting',
    //     permission: '',
    //     meta: { title: 'Manage ' },
    //     icon: '&#xe68a;',
    //     component: main,
    //     children: [
    //         { path: 'user', permission: 'Pages.Users', meta: { title: 'Users' }, name: 'user', component: () => import('../views/setting/user/user.vue') },

    //     ]
    // },
    {
        path: '/dashboard',
        name: 'Home2',
        permission: '',
        icon: '<svg xmlns="http://www.w3.org/2000/svg" width="25" height="25" viewBox="0 0 24.612 24.614"><defs></defs><path class="a" d="M23.951,10.706l0,0L13.908.664a2.265,2.265,0,0,0-3.2,0L.668,10.7l-.01.01A2.265,2.265,0,0,0,2.166,14.57q.035,0,.07,0h.4v7.389a2.655,2.655,0,0,0,2.652,2.652H9.216a.721.721,0,0,0,.721-.721V18.1a1.211,1.211,0,0,1,1.21-1.21h2.317a1.211,1.211,0,0,1,1.21,1.21v5.793a.721.721,0,0,0,.721.721h3.928a2.655,2.655,0,0,0,2.652-2.652V14.573h.371a2.266,2.266,0,0,0,1.6-3.867Zm0,0" transform="translate(0 0)"/></svg>',
        meta: { title: 'HomePage' },
        component: main,
        children: [
            { path: '', permission: undefined, meta: { title: 'HomePage' }, name: '', component: () => import('../views/dashboard/dashboard.vue') },
        ]
    },
    {
        path: '/dashboard',
        name: 'Dashboard.DashboardView',
        permission: '',
        icon: '<svg xmlns="http://www.w3.org/2000/svg" width="25 " height="25" viewBox="0 0 247.12 277.66"><defs></defs><path d="M687.25,532.49c3.19,6,8.81,8.72,14.59,11.1,8.16,3.35,16.6,6,24.58,9.82a80.8,80.8,0,0,1,11.11,6.16c-28.47,3.27-49.12,17.29-59,44.18s-3,50.86,16.82,71.8c-4,.48-7.81,1.05-11.6,1.36-8.83.7-17.66,1.28-26.53,1.26-17.31,0-34.39-2.37-51.41-5.21-15.58-2.61-31-6-45.82-11.55-2.19-.81-4.3-1.91-6.53-2.57-3.86-1.16-5.54-4-6.54-7.57-1.43-5.1-.8-10.25-.37-15.35,1.37-16.12,3.55-32.11,8-47.72a117.64,117.64,0,0,1,6.67-17.67c4.27-9.11,12-14,21-17.07,10.57-3.64,21.1-7.33,30.83-13,3.39-2,6.61-4.1,8.62-7.91,3,6.21,5.93,12.27,9.95,17.72a47.86,47.86,0,0,0,9.53,9.95c8.74,6.58,18.29,6.05,26.94-1.32,6.92-5.9,11.82-13.34,16.24-21.15C685.34,536.08,686.25,534.32,687.25,532.49Z" transform="translate(-546.07 -401)"/><path class="cls-1" d="M706.64,466.65a67.82,67.82,0,0,0,2.16-18.22c-.18-14.05-5.63-25.32-17.41-33.31-7.12-4.83-15.32-7.09-23.33-9.78-5.69-1.92-11.62-2.9-17.44-4.34h-4.88c-1,.67-2.22,0-3.26.45-6.92,1-12.71,4.52-18.19,8.51-9.26,6.74-16.7,15-19.4,26.49-2,8.36-1.11,16.87-.06,25.32.36,2.91,1.7,6-1.21,8.49a3.33,3.33,0,0,0-.71,2,36.35,36.35,0,0,0,4,21.17,18.89,18.89,0,0,1,1.93,4.65c3.62,16.81,12,30.63,26,40.75,12,8.67,24.64,9.15,37.25,1.54,16.27-9.81,25.59-24.68,30.05-42.87a5.8,5.8,0,0,1,.68-2.31c3-3.35,3.76-7.57,4.55-11.7.93-4.84,2.13-9.79-.77-14.47A2.88,2.88,0,0,1,706.64,466.65ZM699.37,475c.84.45.42,1.28.37,2a60.7,60.7,0,0,1-2,10.07,2.45,2.45,0,0,1-.5,1.23c-3.38,2.86-3.1,7.15-4.28,10.85-4.34,13.65-11.8,25.1-23.89,33.1q-16,10.61-30.95-1.55A58.6,58.6,0,0,1,616.9,495a15,15,0,0,0-2.54-5.81c-2.54-3.74-2.67-8.32-3.29-12.68-.1-.7.43-.88,1-1.09,2.71-1.08,3.13-3.24,2.66-5.78-1.08-5.77-1.59-11.61-2.26-17.43-1.83-15.69,5-27,17.35-35.68,1.55-1.09,3.21-2,4.79-3.05,5.59-3.66,11.69-4.42,18.12-3.2a126.3,126.3,0,0,1,27.82,8.6c13.56,6,19.57,15.44,19.92,31.62a61.58,61.58,0,0,1-2.93,17.32C696.61,470.61,696.08,473.28,699.37,475Z" transform="translate(-546.07 -401)"/><path class="cls-1" d="M738.66,569.94c-30.13.27-54.76,25.07-54.27,54.63.5,30.28,25,54.44,54.8,54.09s54.34-25,54-54.62C792.85,594,768.28,569.67,738.66,569.94Zm38.25,39.38-23.95,24q-8.34,8.34-16.68,16.68c-2.56,2.54-3.76,2.53-6.31,0q-12-12.11-24.08-24.23c-2.43-2.45-2.44-3.72-.08-6.13,1.58-1.61,3.19-3.2,4.79-4.79a3.24,3.24,0,0,1,2.39-1.27c1.63-.08,2.48.9,3.37,1.79,5.08,5.14,10.22,10.23,15.22,15.44,1.24,1.29,1.89,1.25,3.13,0q15.84-16,31.8-31.84c3-3,3.85-2.95,6.88.08q1.74,1.71,3.46,3.44C779.83,605.44,779.85,606.38,776.91,609.32Z" transform="translate(-546.07 -401)"/></svg>',
        meta: { title: 'Dashboard' },
        component: main,
        children: [
            { path: '', permission: "Pages.Users", hidden : true, meta: { title: 'HomePage' }, name: 'Dashboard.Home', component: () => import('../views/dashboard/dashboard.vue') },
            { path: 'tenant', permission: "Pages.Tenants", meta: { title: 'Tenant' }, name: 'Dashboard.TenantView', component: () => import('../views/setting/tenant/tenant.vue') },
            { path: 'role', permission: "Pages.Roles", meta: { title: 'Role' }, name: 'Dashboard.RoleView', component: () => import('../views/setting/role/role.vue') },
            { path: 'user', permission: "Pages.Users", meta: { title: 'User' }, name: 'Dashboard.UserView', component: () => import('../views/setting/user/user.vue') },
            { path: 'email', permission: "Pages.Users", meta: { title: 'Email' }, name: 'email', component: () => import('../views/email/email.vue') }
        ]
    },
    {
        path: '/information',
        name: 'information',
        permission: '',
        icon: '<svg xmlns="http://www.w3.org/20a00/svg" width="25" height="25" viewBox="0 0 24.312 24.312"><g transform="translate(-2.25 -2.25)"><path d="M17.841,22.44V14.625H14.368v1.737H16.1V22.44H13.5v1.737h6.946V22.44Z" transform="translate(-2.567 -2.824)"/><path d="M17.615,7.875a1.3,1.3,0,1,0,1.3,1.3A1.3,1.3,0,0,0,17.615,7.875Z" transform="translate(-3.209 -1.284)"/><path d="M14.406,26.562A12.156,12.156,0,1,1,26.562,14.406,12.156,12.156,0,0,1,14.406,26.562Zm0-22.576A10.419,10.419,0,1,0,24.826,14.406,10.419,10.419,0,0,0,14.406,3.987Z"/></g></svg>',
        meta: { title: 'Information' },
        component: main,
        children: [
            //news
            {
                path: 'news', permission: undefined, meta: { title: 'News' }, name: 'news', component: () => import('../views/information/news/news.vue')
            },
            {
                path: 'news/create-news', hidden: true, permission: undefined, meta: { title: 'Create News' }, name: 'CreateNews', component: () => import('../views/information/news/components/createEditNews.vue')
            },
            {
                path: 'news/:newsId&:commentId', hidden: true, permission: undefined, meta: { title: 'News Detail' }, name: 'NewsDetailEntity', component: () => import('../views/information/news/components/newsDetail.vue')
            },
            {
                path: 'news/:newsId', hidden: true, permission: undefined, meta: { title: 'News Detail' }, name: 'NewsDetail', component: () => import('../views/information/news/components/newsDetail.vue')
            },
            {
                path: 'news/edit-news/:newsId', hidden: true, permission: undefined, meta: { title: 'Edit News' }, name: 'EditNews', component: () => import('../views/information/news/components/createEditNews.vue')
            },
            //events
            {
                path: 'events', permission: undefined, meta: { title: 'Events' }, name: 'events', component: () => import('../views/information/events/events.vue')
            },
            {
                path: 'events/create-events', hidden: true, permission: undefined, meta: { title: 'Create Event' }, name: 'CreateEvent', component: () => import('../views/information/events/components/createEditEvent.vue')
            },
            {
                path: 'events/:eventId&:commentId', hidden: true, permission: undefined, meta: { title: 'Event Detail' }, name: 'EventDetailEntity', component: () => import('../views/information/events/components/eventDetail.vue')
            },
            {
                path: 'events/:eventId', hidden: true, permission: undefined, meta: { title: 'Event Detail' }, name: 'EventDetail', component: () => import('../views/information/events/components/eventDetail.vue')
            },
            {
                path: 'events/edit-events/:eventId', hidden: true, permission: undefined, meta: { title: 'Event News' }, name: 'EditEvent', component: () => import('../views/information/events/components/createEditEvent.vue')
            },
            //guideline
            {
                path: 'guidelines', permission: undefined, meta: { title: 'Guidelines' }, name: 'guidelines', component: () => import('../views/information/guidelines/guidelines.vue')
            },
            {
                path: 'guidelines/create-guide', hidden: true, permission: undefined, meta: { title: 'Create Guide' }, name: 'CreateGuide', component: () => import('../views/information/guidelines/components/createEditGuide.vue')
            },
            {
                path: 'guidelines/:guideId&:commentId', hidden: true, permission: undefined, meta: { title: 'Guide Detail' }, name: 'GuideDetailEntity', component: () => import('../views/information/guidelines/components/guideDetail.vue')
            },
            {
                path: 'guidelines/:guideId', hidden: true, permission: undefined, meta: { title: 'Guide Detail' }, name: 'GuideDetail', component: () => import('../views/information/guidelines/components/guideDetail.vue')
            },
            {
                path: 'guidelines/create-guide', hidden: true, permission: undefined, meta: { title: 'Create Guide' }, name: 'CreateGuide', component: () => import('../views/information/guidelines/components/createEditGuide.vue')
            },
            {
                path: 'guidelines/edit-guide/:guideId', hidden: true, permission: undefined, meta: { title: 'Guide News' }, name: 'EditGuide', component: () => import('../views/information/guidelines/components/createEditGuide.vue')
            },
            //policy
            {
                path: 'policies',
                permission: undefined,
                meta: { title: 'Policies' },
                name: 'policies',
                component: () => import('../views/information/policies/policies.vue'),
            },
            {
                path: 'policies/create-policy', hidden: true, permission: undefined, meta: { title: 'Create Policy' }, name: 'createPolicy', component: () => import('../views/information/policies/components/editPolicies.vue')
            },
            {
                path: 'policies/edit-policy/:policyId', hidden: true, permission: undefined, meta: { title: 'Edit Policy' }, name: 'editPolicy', component: () => import('../views/information/policies/components/editPolicies.vue')
            },
            {
                path: 'policies/:policyId&:commentId', hidden: true, permission: undefined, meta: { title: 'Policy Detail' }, name: 'policyDetailEntity', component: () => import('../views/information/policies/components/policyDetail.vue')
            },
            {
                path: 'policies/:policyId', hidden: true, permission: undefined, meta: { title: 'Policy Detail' }, name: 'policyDetail', component: () => import('../views/information/policies/components/policyDetail.vue')
            },

        ]
    },
    {
        path: '/user-calendar',
        name: 'UserCalendar',
        permission: '',
        icon: '<svg xmlns="http://www.w3.org/2000/svg" width="25" height="25" viewBox="0 0 23.441 20.575"><path d="M20.694,1.607H19.6V0H17.764V1.607H5.677V0H3.846V1.607h-1.1A2.6,2.6,0,0,0,0,4.019V18.164a2.6,2.6,0,0,0,2.747,2.411H20.694a2.6,2.6,0,0,0,2.747-2.411V4.019A2.6,2.6,0,0,0,20.694,1.607Zm.916,16.556a.866.866,0,0,1-.916.8H2.747a.866.866,0,0,1-.916-.8V7.555H21.61Zm0-12.216H1.831V4.019a.866.866,0,0,1,.916-.8h1.1V4.822H5.677V3.215H17.764V4.822H19.6V3.215h1.1a.866.866,0,0,1,.916.8Z" transform="translate(0)"/><g transform="translate(3.054 9.243)"><rect width="1.607" height="1.607"/></g><g transform="translate(7.216 9.243)"><rect width="1.607" height="1.607"/></g><g transform="translate(10.917 9.243)"><rect width="1.607" height="1.607"/></g><g transform="translate(14.618 9.243)"><rect width="1.607" height="1.607"/></g><g transform="translate(18.78 9.243)"><rect width="1.607" height="1.607"/></g><g transform="translate(3.054 12.458)"><rect width="1.607" height="1.607"/></g><g transform="translate(7.216 12.458)"><rect width="1.607" height="1.607"/></g><g transform="translate(10.917 12.458)"><rect width="1.607" height="1.607"/></g><g transform="translate(14.618 12.458)"><rect width="1.607" height="1.607"/></g><g transform="translate(3.054 15.672)"><rect width="1.607" height="1.607"/></g><g transform="translate(7.216 15.672)"><rect width="1.607" height="1.607"/></g><g transform="translate(10.917 15.672)"><rect width="1.607" height="1.607"/></g><g transform="translate(14.618 15.672)"><rect width="1.607" height="1.607"/></g><g transform="translate(18.78 12.458)"><rect width="1.607" height="1.607"/></g></svg>',
        meta: { title: 'User Calendar' },
        component: main,
        children: [
            { path: '', permission: undefined, meta: { title: 'Calendar' }, name: '', component: () => import('../views/calendar/calendar.vue') },
        ]
    },
    // {
    //     path: '/blog',
    //     name: 'Blog',
    //     permission: '',
    //     meta: { title: 'blog' },
    //     component: main,
        //
    //     children: [
            // my blog
            // {
            //     path: 'myblog', permission: undefined, meta: { title: 'MyBlog' }, name: 'myblog', component: () => import('../views/blog/my-blog/myblog.vue')
            // },
            // {
            //     path: 'myblog/create-myblog', hidden: true, permission: undefined, meta: { title: 'Create Blog' }, name: 'CreateBlog', component: () => import('../views/blog/my-blog/components/createEdit-myblog.vue')
            // },
            // {
            //     path: 'myblog/:blogId', hidden: true, permission: undefined, meta: { title: 'Myblog View' }, name: 'MyblogView', component: () => import('../views/blog/list-blog/components/detailListblog.vue')
            // },
            // {
            //     path: 'myblog/edit-myblog/:myblogId', hidden: true, permission: undefined, meta: { title: 'Edit Myblog' }, name: 'EditMyblog', component: () => import('../views/blog/my-blog/components/createEdit-myblog.vue')
            // },
            // {
            //     path: 'myblog/edit-inforblog', hidden: true, permission: undefined, meta: { title: 'Edit InforBlog' }, name: 'EditInforBlog', component: () => import('../views/blog/my-blog/components/edit-inforBlog.vue')
            // },
            // {
            //     path: 'myblog/blog/:userId', hidden: true, permission: undefined, meta: { title: 'MyBlog' }, name: 'userBlog', component: () => import('../views/blog/my-blog/myblog.vue')
            // },
            // list blog
    //         {
    //             path: 'listblog', permission: undefined, meta: { title: 'ListBlog' }, name: 'listblog', component: () => import('../views/blog/list-blog/listblog.vue')
    //         },
    //         {
    //             path: 'listblog/:blogId', hidden: true, permission: undefined, meta: { title: 'Blog Detail' }, name: 'DetailListBlog', component: () => import('../views/blog/list-blog/components/detailListblog.vue')
    //         },
    //     ]
    // },
    // Giao việc
    {
        path: '/task',
        name: 'task',
        permission: '',
        icon: '<svg xmlns="http://www.w3.org/2000/svg" width="25" height="25" viewBox="0 0 22.955 20.175"><path d="M22.287,2.69h-6.1V2.017A2.02,2.02,0,0,0,14.168,0H8.788A2.02,2.02,0,0,0,6.77,2.017V2.69H.673A.674.674,0,0,0,0,3.362V18.157a2.02,2.02,0,0,0,2.017,2.017h18.92a2.02,2.02,0,0,0,2.017-2.017V3.374a.653.653,0,0,0-.668-.684ZM8.115,2.017a.673.673,0,0,1,.672-.672h5.38a.673.673,0,0,1,.672.672V2.69H8.115ZM21.349,4.035,19.261,10.3a.671.671,0,0,1-.638.46H14.84v-.672a.672.672,0,0,0-.672-.672H8.788a.672.672,0,0,0-.672.672v.672H4.332a.671.671,0,0,1-.638-.46L1.606,4.035ZM13.5,10.76V12.1H9.46V10.76Zm8.115,7.4a.673.673,0,0,1-.672.672H2.018a.673.673,0,0,1-.672-.672V7.507l1.073,3.219A2.015,2.015,0,0,0,4.332,12.1H8.115v.672a.672.672,0,0,0,.672.672h5.38a.672.672,0,0,0,.672-.672V12.1h3.783a2.015,2.015,0,0,0,1.914-1.379L21.61,7.507Zm0,0" transform="translate(0 0)"/></svg>',
        meta: { title: 'task' },
        component: main,
        children: [
            { path: '', permission: undefined, meta: { title: 'task' }, name: '', component: () => import('../views/task/task.vue') },
            {
              path: 'addTask', hidden: true, permission: undefined, meta: { title: 'Add Task' }, name: 'addTask', component: () => import('../views/task/components/addTask.vue')
            },
            {
                path: 'myTask/:jobId', hidden: true, permission: undefined, meta: { title: 'My Task' }, name: 'myTask', component: () => import('../views/task/components/addTask.vue')
            },
            {
                path: 'createTask/:jobId', hidden: true, permission: undefined, meta: { title: 'Created Task' }, name: 'createTask', component: () => import('../views/task/components/addTask.vue')
            },

           
        ]
    },
    
    // Check in
    // {
    //     path: '/checkin',
    //     name: 'checkin',
    //     permission: '',
    //     meta: { title: 'checkin' },
    //     component: main,
    //     children: [
    //         { path: '', permission: undefined, meta: { title: 'checkin' }, name: '', component: () => import('../views/checkin/checkin.vue') },
    //     ]
    // },
     // Extension
     {
        path: '/extension',
        name: 'Extension',
        permission: '',
        icon: '<svg xmlns="http://www.w3.org/2000/svg" width="25" height="25" viewBox="0 0 24.236 20.652"><g transform="translate(0.15 0.15)"><path class="a" d="M23.537,3.731H17.553V1.7A1.866,1.866,0,0,0,15.559,0H8.378A1.866,1.866,0,0,0,6.383,1.7V3.731H.4A.373.373,0,0,0,0,4.07V18.656a1.866,1.866,0,0,0,1.995,1.7H21.942a1.866,1.866,0,0,0,1.995-1.7V4.07A.373.373,0,0,0,23.537,3.731ZM7.181,1.7A1.12,1.12,0,0,1,8.378.678h7.181a1.12,1.12,0,0,1,1.2,1.018V3.731h-.8V1.7a.373.373,0,0,0-.4-.339H8.378a.373.373,0,0,0-.4.339V3.731h-.8Zm7.979.339v1.7H8.777v-1.7ZM6.782,15.6v4.07h-1.6V15.6a.322.322,0,0,0-.143-.261l-2.25-1.594V12.035l1.2-.814v1.669a.325.325,0,0,0,.16.271l1.6,1.018a.455.455,0,0,0,.478,0l1.6-1.018a.325.325,0,0,0,.16-.271V11.221l1.2.814v1.713l-2.25,1.594A.323.323,0,0,0,6.782,15.6Zm8.378,4.07H11.17V19H15.16Zm-2.511-6.006-.632-.537.651-1.936h.994l.651,1.936-.632.537a.315.315,0,0,0-.117.24v4.41h-.8v-4.41A.315.315,0,0,0,12.649,13.667Zm8.1,6.006h-1.2V19h-.8v.678h-1.2v-.538l1.479-1.257a.315.315,0,0,0,.117-.24.315.315,0,0,0,.117.24l1.479,1.257ZM18.75,17.159l-1.2-1.018V13.369l.963-.819h.234Zm4.388,1.5a1.12,1.12,0,0,1-1.2,1.018h-.4V19a.315.315,0,0,0-.117-.24l-1.314-1.117,1.314-1.117a.315.315,0,0,0,.117-.24V13.229a.315.315,0,0,0-.117-.24l-1.2-1.018a.435.435,0,0,0-.282-.1h-1.6a.435.435,0,0,0-.282.1l-1.2,1.018a.315.315,0,0,0-.117.24v3.053a.315.315,0,0,0,.117.24l1.314,1.117-1.314,1.117a.315.315,0,0,0-.117.24v.678h-.8V18.656a.373.373,0,0,0-.4-.339h-1.2V14.047l.681-.579a.306.306,0,0,0,.1-.333l-.8-2.374a.4.4,0,0,0-.384-.246h-1.6a.4.4,0,0,0-.384.246l-.8,2.374a.305.305,0,0,0,.1.333l.681.579v4.269h-1.2a.373.373,0,0,0-.4.339v1.018H7.58V15.762l2.25-1.594a.322.322,0,0,0,.143-.261V11.872a.323.323,0,0,0-.15-.265L7.829,10.25a.46.46,0,0,0-.422-.041.341.341,0,0,0-.226.306v2.2l-1.2.763-1.2-.763v-2.2a.34.34,0,0,0-.226-.306.458.458,0,0,0-.422.041L2.144,11.607a.322.322,0,0,0-.15.265v2.035a.322.322,0,0,0,.143.261l2.25,1.594v3.912H1.995A1.12,1.12,0,0,1,.8,18.656V8.819H23.138Zm-3.59-1.5V12.55h.234l.963.819v2.772Zm3.59-9.018H.8V4.41H23.138Z" transform="translate(0 0)"/><g transform="translate(9.973 6.406)"><rect class="a" width="0.798" height="0.798"/></g><g transform="translate(11.569 6.406)"><rect class="a" width="0.798" height="0.798"/></g><g transform="translate(13.165 6.406)"><rect class="a" width="0.798" height="0.798"/></g></g></svg>',
        meta: { title: 'extension' },
        component: main,
        children: [
            // checkin
            {
                path: 'checkin', permission: undefined, meta: { title: 'checkin' }, name: 'checkin', component: () => import('../views/extension/checkin/checkin.vue')
            },
            // unlock timesheets
            {
                path: 'unlockTimesheets', permission: undefined, meta: { title: 'unlock-timesheets' }, name: 'unlockTimesheets', component: () => import('../views/extension/unlock-timesheet/unlock-timesheet.vue')
            },
         
        ]
    },
    {
        path: '/traditional-room',
        name: 'TraditionalRoom',
        permission: '',
        icon: '<svg xmlns="http://www.w3.org/2000/svg" width="25" height="25" viewBox="0 0 24.175 18.865"><g transform="translate(0.1 0.1)"><path class="a" d="M20.911,0H3.064C1.374,0,0,1.07,0,2.385v13.9c0,1.315,1.374,2.385,3.064,2.385H20.911c1.689,0,3.064-1.07,3.064-2.385V2.385C23.975,1.07,22.6,0,20.911,0Zm1.651,16.28a1.505,1.505,0,0,1-1.651,1.286H3.064A1.505,1.505,0,0,1,1.412,16.28V14.275L6.06,11.2a.559.559,0,0,1,.588,0L9.56,13.076a.859.859,0,0,0,.951-.034l6.918-5.394a.535.535,0,0,1,.347-.1.512.512,0,0,1,.331.131l4.457,4.272V16.28Zm0-6.078L19.2,6.981a2.091,2.091,0,0,0-1.355-.536,2.193,2.193,0,0,0-1.419.425L9.966,11.909,7.55,10.348a2.286,2.286,0,0,0-2.406.011L1.412,12.831V2.385A1.505,1.505,0,0,1,3.064,1.1H20.911a1.505,1.505,0,0,1,1.651,1.286Z"/><g transform="translate(4.09 3.087)"><path class="a" d="M91.112,63a2.651,2.651,0,1,0,2.651,2.651A2.654,2.654,0,0,0,91.112,63Zm0,4.2a1.552,1.552,0,1,1,1.552-1.552A1.553,1.553,0,0,1,91.112,67.2Z" transform="translate(-88.461 -62.995)"/></g></g></svg>',
        meta: { title: 'Traditional Room' },
        component: main,
        children: [
            { path: '', permission: undefined, meta: { title: 'TraditionalRoom' }, name: '', component: () => import('../views/traditional-room/traditional-room.vue') },
        ]
    }
    ,
    {
        path: '/traditional-room/manage-traditional-room',
        name: 'EditTraditionalRoom',
        permission: '',
        icon: '<i class="fa fa-home"></i>',
        hidden: true,
        meta: { title: 'Manage Traditional Room' },
        component: main,
        children: [
            { path: '/traditional-room/manage-traditional-room', permission: undefined, meta: { title: 'ManageTraditionalRoom' }, name: 'Manage Traditional Room', component: () => import('../views/traditional-room/components/manage-traditional-event.vue') }
        ]
    }
]
export const routers = [
    loginRouter,
    locking,
    ...appRouters,
    otherRouters
];
