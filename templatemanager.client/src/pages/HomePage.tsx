import * as React from "react";
const { useEffect, useState } = React;
import "../styles/HomePage.css";
import CreateHtmlTemplateForm from "../../components/CreateHtmlTemplateForm";
import EditHtmlTemplateForm from "../../components/EditHtmlTemplateForm";
import GeneratePdfForm from "../../components/GeneratePdfForm"; 

interface Template {
    id: string;
    name: string;
    content: string;
}

function HomePage() {
    const [templates, setTemplates] = useState<Template[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [showCreateForm, setShowCreateForm] = useState(false);
    const [showEditForm, setShowEditForm] = useState(false);
    const [selectedTemplate, setSelectedTemplate] = useState<Template | null>(null);
    const [showPdfForm, setShowPdfForm] = useState(false); 
    const [pdfTemplate, setPdfTemplate] = useState<Template | null>(null);

    const fetchTemplates = async () => {
        setLoading(true);
        setError(null);
        try {
            const response = await fetch("/api/HtmlTemplates/list");
            if (!response.ok) throw new Error("Failed to fetch templates");
            const data = await response.json();
            setTemplates(data);
        } catch (err: any) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchTemplates();
    }, []);

    const handleCreateClick = () => setShowCreateForm(true);
    const handleCreateFormClose = () => setShowCreateForm(false);

    const handleTemplateCreated = (newTemplate: Template) => {
        setTemplates((prev) => [...prev, newTemplate]);
        setShowCreateForm(false);
    };

    const handleEditClick = (template: Template) => {
        setSelectedTemplate(template);
        setShowEditForm(true);
    };
    const handleEditFormClose = () => {
        setShowEditForm(false);
        setSelectedTemplate(null);
    };

    const handleTemplateUpdated = (updatedTemplate: Template) => {
        setTemplates((prev) =>
            prev.map((t) => (t.id === updatedTemplate.id ? updatedTemplate : t))
        );
        setShowEditForm(false);
        setSelectedTemplate(null);
    };

    const handleDeleteClick = async (id: string) => {
        if (!window.confirm("Are you sure you want to delete this template?")) return;
        try {
            const response = await fetch(`/api/HtmlTemplates/${id}`, {
                method: "DELETE",
            });
            if (!response.ok) {
                const errorText = await response.text();
                alert("Failed to delete template: " + errorText);
                return;
            }
            setTemplates((prev) => prev.filter((t) => t.id !== id));
        } catch (err) {
            console.error(err);
            alert("An error occurred while deleting the template.");
        }
    };

    const handleGeneratePdfClick = (template: Template) => {
        setPdfTemplate(template);
        setShowPdfForm(true); 
    };

    const handlePdfFormClose = () => {
        setShowPdfForm(false);
        setPdfTemplate(null);
    };

    if (loading) return <div className="home-page">Loading...</div>;
    if (error) return <div className="home-page">Error: {error}</div>;

    return (
        <div className="home-page">
            <div className="side-panel">
                <div className="app-title">Template Manager</div>
                <button className="panel-btn top-btn">About</button>
                <button className="panel-btn">Settings</button>
                <div className="spacer" />
                <button className="panel-btn logout-btn">Log Out</button>
            </div>

            <div className="main-content">
                <header className="header">
                    <button className="account-button"></button>
                </header>

                <div className="top-buttons">
                    <button className="create-template-btn" onClick={handleCreateClick}>
                        Create New Template
                    </button>
                </div>

                <div className="url-table-container">
                    <table className="url-table">
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Preview</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            {templates.map((t) => (
                                <tr key={t.id}>
                                    <td>{t.name}</td>
                                    <td>{t.content}</td>
                                    <td>
                                        <button onClick={() => handleEditClick(t)}>Edit</button>
                                        <button onClick={() => handleDeleteClick(t.id)}>Delete</button>
                                        <button onClick={() => handleGeneratePdfClick(t)}>
                                            Generate PDF
                                        </button>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            </div>

            {showCreateForm && (
                <CreateHtmlTemplateForm
                    onClose={handleCreateFormClose}
                    onCreated={handleTemplateCreated}
                />
            )}

            {showEditForm && selectedTemplate && (
                <EditHtmlTemplateForm
                    templateId={selectedTemplate.id}
                    initialName={selectedTemplate.name}
                    initialContent={selectedTemplate.content}
                    onClose={handleEditFormClose}
                    onUpdated={handleTemplateUpdated}
                />
            )}

            {showPdfForm && pdfTemplate && (
                <GeneratePdfForm
                    templateId={pdfTemplate.id}
                    templateName={pdfTemplate.name}
                    onClose={handlePdfFormClose}
                />
            )}
        </div>
    );
}

export default HomePage;





