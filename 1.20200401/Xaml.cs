 public partial class AddMeetingView : Window
    {
        private readonly EditCallGroupWindowViewModel _viewModel;

        public static AddMeetingView Instance;
        public static AddMeetingView GetAddMeetingView()
        {
            if (Instance != null)
            {
                Instance.Close();
            }
            Instance = new AddMeetingView();
            return Instance;
        }

        public static AddMeetingView GetAddMeetingView(FavorGroup item)
        {
            if (Instance != null)
            {
                Instance.Close();
            }
            Instance = new AddMeetingView(item);
            return Instance;
        }

        private AddMeetingView()
        {
            InitializeComponent();
            this.Topmost = ConfigManager.Instance.ConfigInfo.Local.IsTopMostMode;
            this.gridWin.MouseLeftButtonDown += (o, e) => DragMove();
            _viewModel = new EditCallGroupWindowViewModel() { CloseAction = Close };
            this.DataContext = _viewModel;
        }

        private AddMeetingView(FavorGroup item)
        {
            InitializeComponent();
            this.gridWin.MouseLeftButtonDown += (o, e) => DragMove();//使窗体能被拖动
            this.listbox.MouseLeftButtonDown += (s, e) => DragMove();//使listbox能被拖动
            _viewModel = new EditCallGroupWindowViewModel(item) { CloseAction = Close };
            this.DataContext = _viewModel;
        }

       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Instance = null;
        }

        private DateTime lastDropTime = DateTime.Now;
        private void EditCallGroupWindow_Drop(object sender, DragEventArgs e)//其它数据往此窗体中拖动
        {
            if (Math.Abs((lastDropTime - DateTime.Now).TotalMilliseconds) < 500)
            {
                CommandCenterLogManager.Debug("EditCallGroupWindow.EditCallGroupWindow_Drop so quick,return");
                return;
            }
           
            lastDropTime = DateTime.Now;
            if (e.Data == null) return;
            var staff = e.Data.GetData(typeof(StaffInfoDTO)) as StaffInfoDTO;
            var interphone = e.Data.GetData(typeof(InterphoneDTO)) as InterphoneDTO;
            var car = e.Data.GetData(typeof(CarInfoDTO)) as CarInfoDTO;
            if (car != null && car.InterphoneId.IsEmpty()) return;
            var smartApp = e.Data.GetData(typeof(ViewSmartAppDTO)) as ViewSmartAppDTO;
            var ssi = e.Data.GetData(typeof(SSIGroupDTO)) as SSIGroupDTO;
            var pttNumberInforextend = e.Data.GetData(typeof(PttNumberInfoBase)) as PttNumberInfoBase;
            var recorder = e.Data.GetData(typeof(RecorderDTO)) as RecorderDTO;
            var favoriteGroupMemberDtoList = e.Data.GetData(typeof(List<FavoriteGroupMemberDto>)) as List<FavoriteGroupMemberDto>;

            FavoriteGroupMemberDto dto = null;
            if (staff != null)//顺序很重要，必须第一个
            {
                if (string.IsNullOrEmpty(staff.DEVICENO))
                {
                    dto = new FavoriteGroupMemberDto();
                }
                else
                {
                    dto = new FavoriteGroupMemberDto()
                    {
                        ID = Guid.NewGuid().ToString(),
                        STAFF_CODE = staff.StaffCode,
                        ENTITY_TYPE = (int)FavoriteTypeEnum.Staff,
                        NAME = staff.Name
                    };
                }
            }
            else if (car != null)
            {
                if (string.IsNullOrEmpty(car.DeviceNum))
                {
                    dto = new FavoriteGroupMemberDto();
                }
                else
                {
                    dto = new FavoriteGroupMemberDto()
                    {
                        ID = Guid.NewGuid().ToString(),
                        CAR_GUID = car.Id,
                        ENTITY_TYPE = (int)FavoriteTypeEnum.Car,
                        NAME = string.IsNullOrWhiteSpace(car.CarNo) ? car.Name : car.CarNo,
                    };
                }
            }
            else if (interphone != null)
            {
                dto = new FavoriteGroupMemberDto()
                {
                    ID = Guid.NewGuid().ToString(),
                    PUCID = interphone.PucID,
                    SYSTEMID = interphone.SystemID,
                    DEVICENUMBER = interphone.DeviceNum,
                    ENTITY_TYPE = (int)FavoriteTypeEnum.Interphone,
                    NAME = string.IsNullOrWhiteSpace(interphone.DeviceName) ? interphone.DeviceNum : interphone.DeviceName
                };
            }
            else if (smartApp != null)
            {
                dto = new FavoriteGroupMemberDto()
                {
                    ID = Guid.NewGuid().ToString(),
                    PUCID = smartApp.PucId,
                    SYSTEMID = smartApp.SystemId,
                    DEVICENUMBER = smartApp.DeviceId,
                    ENTITY_TYPE = (int)FavoriteTypeEnum.SmartOneApp,
                    NAME = smartApp.Name
                };
            }
            else if (ssi != null)
            {
                dto = new FavoriteGroupMemberDto()
                {
                    ID = Guid.NewGuid().ToString(),
                    PUCID = ssi.PucId,
                    SYSTEMID = ssi.SystemID,
                    DEVICENUMBER = ssi.GroupNum,
                    ENTITY_TYPE = (int)FavoriteTypeEnum.Group,
                    NAME = string.IsNullOrWhiteSpace(ssi.GroupName) ? ssi.GroupNum : ssi.GroupName,
                };
            }
            else if (recorder != null)
            {
                dto = new FavoriteGroupMemberDto()
                {
                    ID = Guid.NewGuid().ToString(),
                    PUCID = recorder.Video.PucId,
                    SYSTEMID = recorder.Video.SystemId,
                    DEVICENUMBER = recorder.Video.CameraId,
                    ENTITY_TYPE = (int)FavoriteTypeEnum.Recorder,
                    NAME = string.IsNullOrWhiteSpace(recorder.Name) ? recorder.Video.CameraId : recorder.Name,
                };
            }
            else if (pttNumberInforextend != null)
            {
                if (pttNumberInforextend.NumType == PTTControl.API.Const.NumberType.NUMBERTYPE_EXTERNAL_MOBILE)
                {
                    dto = new FavoriteGroupMemberDto()
                    {
                        ID = Guid.NewGuid().ToString(),
                        PUCID = null,
                        SYSTEMID = null,
                        DEVICENUMBER = pttNumberInforextend.Number,
                        ENTITY_TYPE = (int)FavoriteTypeEnum.Mobile,
                        NAME = pttNumberInforextend.Number,
                    };
                }
                else if (pttNumberInforextend.NumType == PTTControl.API.Const.NumberType.NUMBERTYPE_EXTERNAL_TELEPHONE)
                {
                    dto = new FavoriteGroupMemberDto()
                    {
                        ID = Guid.NewGuid().ToString(),
                        PUCID = null,
                        SYSTEMID = null,
                        DEVICENUMBER = pttNumberInforextend.Number,
                        ENTITY_TYPE = (int)FavoriteTypeEnum.Telephone,
                        NAME = pttNumberInforextend.Number,
                    };
                }
            }
            else if (favoriteGroupMemberDtoList != null)
            {
                foreach (var item in favoriteGroupMemberDtoList)
                {
                    dto = new FavoriteGroupMemberDto()
                    {
                        ID = Guid.NewGuid().ToString(),
                        PUCID = item.PUCID,
                        SYSTEMID = item.SYSTEMID,
                        DEVICENUMBER = item.DEVICENUMBER,
                        ENTITY_TYPE = item.ENTITY_TYPE,
                        NAME = item.NAME,
                        CAR_GUID = item.CAR_GUID,
                        STAFF_CODE = item.STAFF_CODE,
                    };
                    _viewModel.AddDrop(dto);
                }
                return;
            }
            _viewModel.AddDrop(dto);
        }

        private void MeetingName_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.MeetingName.IsReadOnly = false;
        }

        private void MeetingName_LostFocus(object sender, RoutedEventArgs e)
        {
            this.MeetingName.IsReadOnly = true;
        }

        private void listItemContent_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)//当listbox有内容时，按住内容也能拖动窗体
        {
            DragMove();
        }

    }