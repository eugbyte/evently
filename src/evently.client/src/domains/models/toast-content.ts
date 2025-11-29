export enum ToastStatus {
	Success = 0,
	Info,
	Warning,
	Error
}

export const toastStyles = {
	[ToastStatus.Success]: "alert alert-success",
	[ToastStatus.Info]: "alert alert-info",
	[ToastStatus.Warning]: "alert alert-warning",
	[ToastStatus.Error]: "alert alert-error"
};

export class ToastContent {
	constructor(
		public show: boolean,
		public message = "",
		public toastStatus = ToastStatus.Info
	) {}
}
