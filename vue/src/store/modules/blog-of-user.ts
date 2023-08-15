import { Store, Module, ActionContext } from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import User from '../entities/user'
import Blog from '../entities/news'
import Ajax from '../../lib/ajax'
import PageResult from '@/store/entities/page-result';
import ListMutations from './list-mutations'

interface BlogUserState extends ListState<Blog> {
}
class BlogMutations extends ListMutations<User>{
}
class BlogUserModule extends ListModule<BlogUserState, any, Blog>{
    state = {
        title: '',
        typeName: '',
        pageNumber: 1,
        pageSize: 4,
        list: new Array<Blog>(),
        loading:false,
        currentPage:1,
        totalCount: 0
    }
    actions = {
        async detailBlog(context: ActionContext<BlogUserState, any>, payload: any){
            let response = await Ajax.post('/api/services/app/BlogOfUser/DetailBlog?UserId='+payload.id)
            return response.data.result
        },
        
        async updateInfo(context: ActionContext<BlogUserState, any>, payload: any) {
            let reponse = await Ajax.put('/api/services/app/BlogOfUser/UpdateInfo', payload.data);
            return reponse.data.result
        },

    };
    mutations = {
        setCurrentPage(state: BlogUserState, page: number) {
            state.currentPage = page;
        },
        setPageSize(state: BlogUserState, pagesize: number) {
            state.pageSize = pagesize;
        },
    }
}
const blogUserModule = new BlogUserModule();
export default blogUserModule;