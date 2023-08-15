import { Store, Module, ActionContext } from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import Dashboard from '../entities/dashboard'
import Ajax from '../../lib/ajax'
import ListMutations from './list-mutations'

interface DashboardState extends ListState<Dashboard> {
}
class UserMutations extends ListMutations<Dashboard>{

}
class UserModule extends ListModule<DashboardState, any, Dashboard>{
    state = {
        totalCount: 0,
        currentPage: 1,
        pageSize: 10,
        list: new Array<Dashboard>(),
        loading: false,
        editUser: new Dashboard()
    }
    actions = {
        async getLastestInfor(context: ActionContext<DashboardState, any>, payload: any) {
            return await Ajax.get('/api/services/app/DashBoard/GetLatestInformation', { params: payload.data });
        },

        async getAll(context: ActionContext<DashboardState, any>, payload: any) {
            if (payload.data) {
                return await Ajax.get('/api/services/app/DashBoard/GetAll?Title=' + payload.data);
            } else {
                return await Ajax.get('/api/services/app/DashBoard/GetAll');
            }
        },

        async getListImage(context: ActionContext<DashboardState, any>) {
            let response = await Ajax.get('/api/services/app/FaceId/GetListImage');
            return response.data.result
        },
        async decideImage(context: ActionContext<DashboardState, any>, payload: any) {
            let reponse = await Ajax.post('/api/services/app/FaceId/DecideImage', payload.data)
            return reponse.data.result
        },
        async checkIn(context: ActionContext<DashboardState, any>, payload: any) {
            let reponse = await Ajax.post('/api/services/app/FaceId/CheckIn', payload.data)
            return reponse.data.result
        },
        async getAllQuickNews(context: ActionContext<DashboardState, any>,payload: any) {
            let reponse = await Ajax.post('/api/services/app/QuickNews/GetAllQuickNews', payload.data);
            return reponse.data.result
        },
        async changeFormatTextQuickNews(context: ActionContext<DashboardState, any>,payload: any) {
            let reponse = await Ajax.post('/api/services/app/QuickNews/ChangeFormatTextQuickNews', payload.data);
            return reponse.data.result
        },
        async getListHoverContentText(context: ActionContext<DashboardState, any>,payload: any) {
            let reponse = await Ajax.get('/api/services/app/QuickNews/GetListHoverContentText');
            return reponse.data.result
        }
    };
}
const userModule = new UserModule();
export default userModule;