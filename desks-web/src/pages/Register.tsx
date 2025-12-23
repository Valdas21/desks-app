import { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const Register = () => {
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string>();
  const [success, setSuccess] = useState<string>();
    const navigate = useNavigate();

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(undefined);
    setSuccess(undefined);
    try {
      await axios.post("/api/Users/register", {
        firstName,
        lastName,
        email,
        password,
      });
      setSuccess("Registered successfully. Redirecting to login...");
      navigate("/login");
    } catch (err: any) {
      const msg = err?.response?.data ?? err?.message ?? "Register failed";
      setError(String(msg));
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container mt-5" style={{ maxWidth: 520 }}>
      <div className="card">
        <div className="card-body">
          <h4 className="card-title mb-3">Register</h4>
          {error && <div className="alert alert-danger">{error}</div>}
          {success && <div className="alert alert-success">{success}</div>}
          <form onSubmit={onSubmit}>
            <div className="row">
              <div className="col-md-6 mb-3">
                <label className="form-label">First name</label>
                <input
                  className="form-control"
                  value={firstName}
                  onChange={(e) => setFirstName(e.target.value)}
                  required
                />
              </div>
              <div className="col-md-6 mb-3">
                <label className="form-label">Last name</label>
                <input
                  className="form-control"
                  value={lastName}
                  onChange={(e) => setLastName(e.target.value)}
                  required
                />
              </div>
            </div>
            <div className="mb-3">
              <label className="form-label">Email</label>
              <input
                type="email"
                className="form-control"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
            </div>
            <div className="mb-3">
              <label className="form-label">Password</label>
              <input
                type="password"
                className="form-control"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
                minLength={6}
              />
            </div>
            <button className="btn btn-success w-100" disabled={loading} type="submit">
              {loading ? "Creating account..." : "Register"}
            </button>
          </form>
          <div className="mt-3 text-center">
            <a href="/login">Already have an account? Login</a>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Register;