import { Store, Module, ActionContext } from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import User from '../entities/user'
import Task from '../entities/task'
import Ajax from '../../lib/ajax'
import PageResult from '@/store/entities/page-result';
import ListMutations from './list-mutations'

interface TaskState extends ListState<Task> {
}
class NewsMutations extends ListMutations<User>{
}
class TaskModule extends ListModule<TaskState, any, Task>{
    state={
        totalCount:0,
        currentPage:1,
        pageSize:10,
        list: new Array<Task>(),
        loading:false,
        editUser:new Task()
    }
    actions = {
        // async getAllNews(context: ActionContext<TaskState, any>, payload: any) {
        //     context.state.loading = true;
        //     let reponse = await Ajax.post('/api/services/app/New/GetAllNews', payload.data);
        //     context.state.loading = false;
        //     let page = reponse.data.result as PageResult<User>;
        //     return reponse.data.result
        // },

       
        async save(context: any, payload: any){
            let reponse = await Ajax.post('/api/services/app/Job/Save', payload.dataTask);
            return reponse.data.result
        },
        async getAllCreatedJob(context: ActionContext<TaskState, any>, payload: any){
            let response = await Ajax.get('/api/services/app/Job/GetAllCreatedJob',{params:payload.data})
            return response.data.result
        },
        async getAllMyJob(context: ActionContext<TaskState, any>, payload: any){
            let response = await Ajax.get('/api/services/app/Job/GetAllMyJob',{params:payload.data})
            return response.data.result
        },
        async change(context: any, payload: any){
            let respone = await Ajax.post('api/services/app/Job/ChangeStatusJob',payload.data)
            return respone.data.result
        },
        async delete(context: any, payload: any){
            await Ajax.delete('/api/services/app/Job/Delete?Id='+payload.id);
        },
        async remind(context: any, payload: any){
            let reponse = await Ajax.post('/api/services/app/Job/Remind', payload.data);
            return reponse.data.result
        },
        async getMyJob(context:any, payload:any){
            let reponse = await Ajax.get('/api/services/app/Job/GetMyJob?id='+payload.id);
            return reponse.data.result
        },
        async getCreatedJob(context:any, payload:any){
            let reponse = await Ajax.get('/api/services/app/Job/GetCreatedJob?id='+payload.id);
            return reponse.data.result
        },
        async viewJobInDashboard(context: any, payload: any) {
            let reponse = await Ajax.get('/api/services/app/Job/ViewJobInDashboard');
            return reponse.data.result
        },
        async changeOrderJob(context: any, payload: any){
            let reponse = await Ajax.post('/api/services/app/Job/ChangeOrderJob', payload.data);
            return reponse.data.result
        },
    };
    mutations = {
        setCurrentPage(state: TaskState, page: number) {
            state.currentPage = page;
        },
        setPageSize(state: TaskState, pagesize: number) {
            state.pageSize = pagesize;
        },
    }
}
const taskModule = new TaskModule();
export default taskModule;