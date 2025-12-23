import { useState, useEffect } from "react";
import axios from "axios";
import Calendar from "react-calendar";
import "react-calendar/dist/Calendar.css";
import NavBar from "../components/NavBar";

function ListGroupDesks() {
  const [loading, setLoading] = useState(true);
  const [desks, setDesks] = useState<Desk[]>([]);
  const [startDate, setStartDate] = useState<Date>(new Date());
  const [endDate, setEndDate] = useState<Date>(new Date());
  const [hasRangeSelected, setHasRangeSelected] = useState<boolean>(false);

  const [selectedDeskId, setSelectedDeskId] = useState<number>();
  const [reserveStart, setReserveStart] = useState<Date | null>(null);
  const [reserveEnd, setReserveEnd] = useState<Date | null>(null);
  const [reservationError, setReservationError] = useState<string>();
  const [error, setError] = useState<boolean>(false);
  const [cancelError, setCancelError] = useState<boolean>(false);
  const [cancelReservationError, setCancelReservationError] =
    useState<string>();
  const [cancelDate, setCancelDate] = useState<Date | null>(null);
  const [reservationId, setReservationId] = useState<number | null>(null);
  const [selectedReservation, setSelectedReservation] =
    useState<Desk["currentReservation"]>();
  const userId = Number(localStorage.getItem("userId"));

  //cia padejo DI, nes Calendar component'as visada parinkdavo neteisinga laika (start date buvo viena diena ankstesne)
  const toLocalDate = (d: Date) => {
    const y = d.getFullYear();
    const m = String(d.getMonth() + 1).padStart(2, "0");
    const day = String(d.getDate()).padStart(2, "0");
    return `${y}-${m}-${day}`;
  };

  type Desk = {
    id: number;
    status: DeskStatus;
    isUnderMaintenance: boolean;
    message?: string;
    currentReservation?: {
      id: number;
      startDate: string;
      endDate: string;
      firstName: string;
      lastName: string;
      userId: number;
    };
  };
  type DeskStatus = 0 | 1 | 2;

  const statusString: Record<DeskStatus, string> = {
    0: "Open",
    1: "Reserved",
    2: "Maintenance",
  };

  const rowClassByStatus: Record<DeskStatus, string> = {
    0: "table-success",
    1: "table-warning",
    2: "table-danger",
  };

  const getHoverText = (desk: Desk): string => {
    if (desk.status === 1 && desk.currentReservation) {
      return `Reserved by ${desk.currentReservation.firstName} ${
        desk.currentReservation.lastName
      } from ${new Date(
        desk.currentReservation.startDate
      ).toLocaleDateString()} to ${new Date(
        desk.currentReservation.endDate
      ).toLocaleDateString()}`;
    }
    if (desk.status === 2) {
      return desk.message || "Under maintenance";
    }
    return "";
  };

  const loadDesks = async () => {
    setLoading(true);
    try {
      const response = await axios.get<Desk[]>("/api/desks", {
        params: {
          start: toLocalDate(startDate),
          end: toLocalDate(endDate),
        },
      });
      setDesks(response.data);
      console.log(response.data);
    } catch (error) {
      console.error("Error loading desks", error);
    } finally {
      setLoading(false);
    }
  };
  useEffect(() => {
    if (hasRangeSelected) {
      loadDesks();
    }
  }, [startDate, endDate, hasRangeSelected]);

  const confirmReservation = async () => {
    try {
      await axios.post("/api/reservations", {
        userId: userId,
        deskId: selectedDeskId,
        startDate: reserveStart ? toLocalDate(reserveStart) : undefined,
        endDate: reserveEnd ? toLocalDate(reserveEnd) : undefined,
      });
      setError(false);
      await loadDesks();
    } catch (err: any) {
      const msg = err.response.data;
      setReservationError(msg);
      setError(true);
    }
  };

  const cancelThisDay = async (reservationId: number) => {
    try {
      if (!cancelDate) {
        setCancelReservationError("Pick a date to cancel.");
        setCancelError(true);
        return;
      }

      await axios.put(`/api/reservations/${reservationId}/cancel-day`, null, {
        params: {
          userId,
          day: cancelDate ? toLocalDate(cancelDate) : undefined,
        },
      });

      setCancelError(false);
      setCancelReservationError(undefined);
      await loadDesks();
    } catch (err: any) {
      const msg = err?.response?.data ?? err?.message ?? "Cancel failed";
      setCancelReservationError(String(msg));
      setCancelError(true);
    }
  };

  const cancelWhole = async (reservationId: number) => {
    try {
      await axios.delete(`/api/reservations/${reservationId}`, {
        params: { userId },
      });

      setCancelError(false);
      setCancelReservationError(undefined);
      await loadDesks();
    } catch (err: any) {
      const msg = err?.response?.data ?? err?.message ?? "Cancel failed";
      setCancelReservationError(String(msg));
      setCancelError(true);
    }
  };

  return (
    <>
      <NavBar />
      <div className="container min-vh-100 d-flex flex-column align-items-center text-center">
        <div className="row justify-content-center w-100 gap-3">
          <div className="col-12 col-md-5 col-lg-4">
            <div className="mb-3">
              <h5 className="mb-2">Select date range</h5>
              <Calendar
                selectRange
                value={[startDate, endDate]}
                onChange={(value) => {
                  if (Array.isArray(value) && value.length === 2) {
                    setStartDate(value[0] as Date);
                    setEndDate(value[1] as Date);
                  }
                  setHasRangeSelected(true);
                }}
              />
              <div className="mt-2">
                <strong>Selected:</strong> {startDate.toLocaleString()} -{" "}
                {endDate.toLocaleString()}
              </div>
            </div>
          </div>
          <div className="col-12 col-md-7 col-lg-6">
            {!hasRangeSelected && (
              <p className="text-muted">Pick a date range to view desks.</p>
            )}

            {loading && hasRangeSelected && (
              <div className="d-flex justify-content-center">
                <div className="spinner-border" role="status">
                  <span className="visually-hidden">Loading...</span>
                </div>
              </div>
            )}
            {hasRangeSelected && desks.length === 0 && !loading && (
              <p>No desks available</p>
            )}
            <table className="table w-auto mx-auto">
              <thead>
                <tr>
                  <th scope="col">#</th>
                  <th scope="col">Status</th>
                  <th scope="col">Action</th>
                </tr>
              </thead>
              <tbody>
                {hasRangeSelected &&
                  desks.map((desk) => (
                    <tr
                      key={desk.id}
                      className={rowClassByStatus[desk.status]}
                      title={getHoverText(desk)}
                    >
                      <th scope="row">{desk.id}</th>
                      <td>{statusString[desk.status]}</td>
                      <td>
                        {desk.status === 0 && (
                          <button
                            type="button"
                            className="btn btn-primary"
                            onClick={() => setSelectedDeskId(desk.id)}
                            data-bs-toggle="modal"
                            data-bs-target="#reserveModal"
                            data-bs-whatever="@mdo"
                          >
                            Reserve
                          </button>
                        )}
                        {desk.status === 1 &&
                          desk.currentReservation?.userId === userId && (
                            <button
                              type="button"
                              className="btn btn-primary"
                              onClick={() => {
                                setReservationId(desk.currentReservation!.id);
                                setSelectedReservation(
                                  desk.currentReservation!
                                );
                                setCancelDate(null);
                                setCancelError(false);
                                setCancelReservationError(undefined);
                              }}
                              data-bs-toggle="modal"
                              data-bs-target="#cancelModal"
                              data-bs-whatever="@mdo"
                            >
                              Cancel Reservation
                            </button>
                          )}
                      </td>
                    </tr>
                  ))}
              </tbody>
            </table>
            <div
              className="modal fade"
              id="reserveModal"
              tabIndex={-1}
              aria-labelledby="exampleModalLabel"
              aria-hidden="true"
            >
              <div className="modal-dialog">
                <div className="modal-content">
                  <div className="modal-header">
                    <h1 className="modal-title fs-5" id="exampleModalLabel">
                      Select reservation range
                    </h1>
                  </div>
                  <div className="modal-body">
                    {error && (
                      <div className="alert alert-danger" role="alert">
                        {reservationError}
                      </div>
                    )}
                    <Calendar
                      selectRange={true}
                      minDate={startDate}
                      maxDate={endDate}
                      onChange={(value) => {
                        if (Array.isArray(value) && value.length === 2) {
                          setReserveStart(value[0] as Date);
                          setReserveEnd(value[1] as Date);
                        }
                      }}
                    />
                  </div>
                  <div className="modal-footer">
                    <button
                      type="button"
                      className="btn btn-success"
                      onClick={confirmReservation}
                    >
                      Confirm
                    </button>
                    <button
                      type="button"
                      className="btn btn-danger"
                      onClick={() => setError(false)}
                      data-bs-dismiss="modal"
                    >
                      Close
                    </button>
                  </div>
                </div>
              </div>
            </div>
            <div
              className="modal fade"
              id="cancelModal"
              tabIndex={-1}
              aria-labelledby="exampleModalLabel"
              aria-hidden="true"
            >
              <div className="modal-dialog">
                <div className="modal-content">
                  <div className="modal-header">
                    <h1 className="modal-title fs-5" id="exampleModalLabel">
                      Select cancellation option
                    </h1>
                  </div>
                  <div className="modal-body">
                    {cancelError && (
                      <div className="alert alert-danger" role="alert">
                        {cancelReservationError}
                      </div>
                    )}
                    {selectedReservation && (
                      <Calendar
                        minDate={new Date(selectedReservation.startDate)}
                        maxDate={new Date(selectedReservation.endDate)}
                        onChange={(value) => {
                          setCancelDate(value as Date);
                        }}
                      />
                    )}
                  </div>
                  <div className="modal-footer">
                    <button
                      type="button"
                      className="btn btn-success"
                      onClick={() => cancelThisDay(reservationId!)}
                    >
                      Cancel for selected date
                    </button>
                    <button
                      type="button"
                      className="btn btn-success"
                      onClick={() => cancelWhole(reservationId!)}
                    >
                      Cancel whole reservation
                    </button>
                    <button
                      type="button"
                      className="btn btn-danger"
                      onClick={() => setCancelError(false)}
                      data-bs-dismiss="modal"
                    >
                      Close
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
export default ListGroupDesks;
