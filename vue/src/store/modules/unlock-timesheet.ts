import { Store, Module, ActionContext } from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import User from '../entities/user'
import Ajax from '../../lib/ajax'
import PageResult from '@/store/entities/page-result';
import ListMutations from './list-mutations'
import UnlockTimeSheet from '../entities/unlock-timesheet'

interface UnLockTimeSheetState extends ListState<UnlockTimeSheet> {
}
class NewsMutations extends ListMutations<User>{
}
class UnlockTimeSheetModule extends ListModule<UnLockTimeSheetState, any, UnlockTimeSheet>{
    state={
        totalCount:0,
        currentPage:1,
        pageSize:10,
        list: new Array<UnlockTimeSheet>(),
        loading:false,
        editUser:new UnlockTimeSheet()
    }
    actions = {
        async unlockToLogTimesheet(context: any, payload: any){
            let reponse = await Ajax.post('/api/services/app/UnlockTimesheet/UnlockToLogTimesheet');
            return reponse.data.result
        },
        async unlockToApproveTimesheet(context: any, payload: any){
            let reponse = await Ajax.post('/api/services/app/UnlockTimesheet/UnlockToApproveTimesheet');
            return reponse.data.result
        },
        
        async getAllTimesheetLocked(context:any, payload: any){
            let response = await Ajax.get('/api/services/app/UnlockTimesheet/GetAllTimesheetLocked')
            return response.data.result
        },
       
        async getTreasuryMoney(context:any, payload: any){
            let response = await Ajax.get('/api/services/app/UnlockTimesheet/GetTreasuryMoney')
            return response.data.result
        },
        
        async getAllHistory(context:any, payload: any){
            let response = await Ajax.get('/api/services/app/UnlockTimesheet/GetAllHistory')
            return response.data.result
        },
        async unlockTimesheetSaturday(context: any, payload: any){
            let reponse = await Ajax.post('/api/services/app/UnlockTimesheet/UnlockSaturday');
            return reponse.data.result
        },
        async getFunAmountHistories(context:any, payload:any){
            console.log(payload,payload.data);
            let reponse = await Ajax.post('/api/services/app/UnlockTimesheet/GetFunAmountHistories',payload.data);
            context.state.loading = false;
            let page = reponse.data.result as PageResult<any>;
            context.state.totalCount = page.totalCount;
            context.state.list = page.items;
        },
        async topUserUnlock(context:any, payload: any){
            let response = await Ajax.get('/api/services/app/UnlockTimesheet/TopUserUnlock')
            return response.data.result
        },
       
    };
    mutations = {
        setCurrentPage(state: UnLockTimeSheetState, page: number) {
            state.currentPage = page;
        },
        setPageSize(state: UnLockTimeSheetState, pagesize: number) {
            state.pageSize = pagesize;
        },
    }
}
const unlockTimeSheetModule = new UnlockTimeSheetModule();
export default unlockTimeSheetModule;