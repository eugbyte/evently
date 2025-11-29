import { type JSX, type Ref, useEffect, useRef } from "react";
import { Booking } from "~/domains/entities";
import QRCode from "qrcode";

// 1. Define the props for the child component
interface QrDialogProps {
	booking: Booking | null;
	qrDialogRef: Ref<HTMLDialogElement>;
}

export function QrDialog({ qrDialogRef, booking }: QrDialogProps): JSX.Element {
	const canvasRef = useRef<HTMLCanvasElement>(null);
	useEffect(() => {
		if (booking == null || canvasRef.current == null) {
			return;
		}

		const url = new URL(
			`gatherings/${booking.gatheringId}/?bookingId=${booking.bookingId}`,
			window.location.origin
		);
		QRCode.toCanvas(canvasRef.current, url.href, (error) => {
			if (error) {
				console.error(error);
			}
		});
	}, [canvasRef, booking]);

	return (
		<dialog className="modal" ref={qrDialogRef}>
			<div className="modal-box">
				<h3 className="text-lg font-bold">Event Ticket</h3>
				<canvas className="mx-auto block" id="qr-canvas" ref={canvasRef}></canvas>
				<div className="my-2 text-center">
					<code>{booking?.bookingId}</code>
				</div>
				<div className="modal-action">
					<form method="dialog">
						{/* if there is a button in form, it will close the modal */}
						<button className="btn">Close</button>
					</form>
				</div>
			</div>
		</dialog>
	);
}
