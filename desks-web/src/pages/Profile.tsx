import { useEffect, useState } from "react";
import axios from "axios";
import NavBar from "../components/NavBar";

type UserInfo = {
  id: number;
  email?: string | null;
  firstName?: string | null;
  lastName?: string | null;
};

type Reservation = {
  id: number;
  deskId: number;
  startDate: string; 
  endDate: string;   
};

type UserReservationsResponse = {
  user: UserInfo;
  reservations: Reservation[];
};

const formatDate = (iso: string) => {
  const d = iso?.slice(0, 10); 
  if (!d) return "";
  const [y, m, day] = d.split("-").map(Number);
  return new Date(y, (m ?? 1) - 1, day ?? 1).toLocaleDateString();
};

const Profile = () => {
  const [current, setCurrent] = useState<Reservation[]>([]);
  const [previous, setPrevious] = useState<Reservation[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>();

  const userId = Number(localStorage.getItem("userId"));
  const userEmail = localStorage.getItem("email") || "";
  const userFirstName = localStorage.getItem("firstName") || "";
    const userLastName = localStorage.getItem("lastName") || "";

  useEffect(() => {
    const load = async () => {
      try {
        setLoading(true);
        setError(undefined);

        const [currRes, pastRes] = await Promise.all([
          axios.get<UserReservationsResponse>(`/api/Users/${userId}/Reservations-current`),
          axios.get<UserReservationsResponse>(`/api/Users/${userId}/Reservations-past`),
        ]);

        setCurrent(currRes.data?.reservations ?? []);
        setPrevious(pastRes.data?.reservations ?? []);
        
      } catch (err: any) {
        const msg = err?.response?.data ?? err?.message ?? "Failed to load profile";
        setError(String(msg));
      } finally {
        setLoading(false);
      }
    };
    load();
  }, [userId]);

  return (
    <>
      <NavBar />
      <div className="container py-4">
        <h3 className="mb-3">Profile</h3>

        {loading && (
          <div className="d-flex justify-content-center my-4">
            <div className="spinner-border" role="status" />
          </div>
        )}

        {error && <div className="alert alert-danger">{error}</div>}

        {!loading && !error && (
          <>
            <div className="card mb-3">
              <div className="card-body">
                <h5 className="card-title">User</h5>
                <p className="card-text mb-0">
                  <strong>Name:</strong> {userFirstName} {userLastName}
                </p>
                {userEmail && (
                  <p className="card-text mb-0">
                    <strong>Email:</strong> {userEmail}
                  </p>
                )}
              </div>
            </div>

            <div className="row g-3">
              <div className="col-12 col-lg-6">
                <div className="card h-100">
                  <div className="card-body">
                    <h5 className="card-title">Current Reservations</h5>
                    {current.length === 0 ? (
                      <p className="text-muted mb-0">No current reservations.</p>
                    ) : (
                      <ul className="list-group list-group-flush">
                        {current.map((r) => (
                          <li key={r.id} className="list-group-item">
                            <div className="d-flex justify-content-between">
                              <span>Desk #{r.deskId}</span>
                              <span>
                                {formatDate(r.startDate)} — {formatDate(r.endDate)}
                              </span>
                            </div>
                          </li>
                        ))}
                      </ul>
                    )}
                  </div>
                </div>
              </div>

              <div className="col-12 col-lg-6">
                <div className="card h-100">
                  <div className="card-body">
                    <h5 className="card-title">Previous Reservations</h5>
                    {previous.length === 0 ? (
                      <p className="text-muted mb-0">No previous reservations.</p>
                    ) : (
                      <ul className="list-group list-group-flush">
                        {previous.map((r) => (
                          <li key={r.id} className="list-group-item">
                            <div className="d-flex justify-content-between">
                              <span>Desk #{r.deskId}</span>
                              <span>
                                {formatDate(r.startDate)} — {formatDate(r.endDate)}
                              </span>
                            </div>
                          </li>
                        ))}
                      </ul>
                    )}
                  </div>
                </div>
              </div>
            </div>
          </>
        )}
      </div>
    </>
  );
};

export default Profile;
