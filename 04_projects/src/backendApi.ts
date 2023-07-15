import axios from "axios"

const hostBegin = "https://localhost:7093"
const localPath = "D:/01_Synchronized/01_Programming_Files/8ce8792f-dc83-4978-a1d8-1a49c71937ec"

type urlItem =
{
    url: string
}

export type item =
{
    Name: string
    Content: string[]
}

export async function GetItem(repo: string, loca: string) : Promise<item>
{
    const address = hostBegin + "/" + "repoApi" + "/" + repo + "/" + loca

    const response2 = axios.get('/users')
 
    const response = await window.fetch(address, 
    {
        method: 'GET',
        headers:
        {
            'content-type': 'application/json',
        },
    })
    
    const item = await response.json()
    return item as item
}


    //let headers = new Headers();
    //headers.append('Content-Type', 'application/json');
    //headers.append('Content-Type', 'text/plain');
    //headers.append('Content-Type', 'text/plain;charset=UTF-8');
    //headers.append('Access-Control-Allow-Origin', 'http://localhost:8081');
    //headers.append('Access-Control-Allow-Credentials', 'true');

    //const repo = "Notki"
    //const loca = "01-02"

export async function OpenPdf(repo: string, loca: string)
{
    const address = hostBegin + "/" + "commandApi" + "/" + "OpenPdfFile" + "/" + repo + "/" + loca
    InvokeGet(address)
    const loca2 = loca.replace("-", "/")
    const pdfFilePath = localPath + "/" + repo + "/" + loca2 + "/" + "lista.pdf"
    const pdfFilePath2 = "D:/01_Synchronized/01_Programming_Files/8ce8792f-dc83-4978-a1d8-1a49c71937ec/Sprawy/01/02"
    window.open(pdfFilePath2);
}

export async function OpenFolder(repo: string, loca: string)
{
    const address = hostBegin + "/" + "commandApi" + "/" + "OpenFolder" + "/" + repo + "/" + loca
    let jsonResponse = await InvokeGet(address)
}

export async function OpenTextFile(repo: string, loca: string)
{
    const address = hostBegin + "/" + "commandApi" + "/" + "OpenTextFile" + "/" + repo + "/" + loca
    InvokeGet(address)
}

export async function OpenConfigFile(repo: string, loca: string)
{
    const address = hostBegin + "/" + "commandApi" + "/" + "OpenConfigFile" + "/" + repo + "/" + loca
    InvokeGet(address)
}

export async function OpenGoogleDoc(repo: string, loca: string)
{
    const address = hostBegin + "/" + "commandApi" + "/" + "OpenGoogleDoc" + "/" + repo + "/" + loca
    const jsonResponse = await InvokeGet(address)
    window.open(jsonResponse.url);
}

export async function InvokeGet(address: string) : Promise<any>
{ 
    const response = await window.fetch(address, 
    {
        method: 'GET',
        headers:
        {
            'content-type': 'application/json',
        },
    })
    
    const result = await response.json()
    //console.log("result " + result)
    return result
}
