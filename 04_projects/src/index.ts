import { v4 as uuidV4 } from 'uuid'
import * as backendApi from "./backendApi";
import * as contentOp from "./contentOperations";
//import {express as express} from 'express'

const contentTable = document.querySelector<HTMLTableElement>('#contentTable')



const form = document.getElementById("input-form") as HTMLFormElement | null
const inputRepo = document.querySelector<HTMLInputElement>("#input-repo")
const inputLoca = document.querySelector<HTMLInputElement>("#input-loca")
const folderButton = document.getElementById('folderButton');
const contentFileButton = document.getElementById('contentFileButton');
const configFileButton = document.getElementById('configFileButton');
const pdfFileButton = document.getElementById('pdfFileButton');
const googleDocButton = document.getElementById('googleDocButton');
//const inputUser = document.querySelector<HTMLInputElement>("#input-user")

initialize()

form?.addEventListener("submit", e => FormListener(e))
folderButton?.addEventListener("click", FolderButtonListener)
contentFileButton?.addEventListener("click", ContentFileButtonListener)
configFileButton?.addEventListener("click", ConfigFileButtonListener)
pdfFileButton?.addEventListener("click", PdfFileButtonListener)
googleDocButton?.addEventListener("click", GoogleDocButtonListener)

async function PdfFileButtonListener()
{
    const item = loadItem()
    backendApi.OpenPdf(item.repo, item.loca)
}

async function GoogleDocButtonListener()
{
    const item = loadItem()
    backendApi.OpenGoogleDoc(item.repo, item.loca)
}

async function FolderButtonListener()
{
    const item = loadItem()
    backendApi.OpenFolder(item.repo, item.loca)
}

async function ContentFileButtonListener()
{
    const item = loadItem()
    backendApi.OpenTextFile(item.repo, item.loca)
}

async function ConfigFileButtonListener()
{
    const item = loadItem()
    backendApi.OpenConfigFile(item.repo, item.loca)
}

async function FormListener(e: SubmitEvent)
{
    e.preventDefault()

    if (inputRepo?.value == "" || inputRepo?.value == null) return
    if (inputLoca?.value == "" || inputLoca?.value == null) return

    await fullfillContent(inputRepo.value, inputLoca.value)
}

async function fullfillContent(repo: string, loca: string)
{
    contentOp.removeAllRows(contentTable)
    const item = await backendApi.GetItem(repo, loca)
    contentOp.getAll5(contentTable, item)
    saveItem(repo, loca)
}

async function initialize()
{
    const item = loadItem()
    if (item == null) return
    if (inputRepo == null) return
    if (inputLoca == null) return
    inputRepo.value = item.repo
    inputLoca.value = item.loca
    await fullfillContent(item.repo, item.loca)
}

function saveItem(repo: string, loca: string)
{
    const item = {repo, loca}
    localStorage.setItem("ITEM", JSON.stringify(item))
    
}

function loadItem()
{
    const itemJson = localStorage.getItem("ITEM")
    if (itemJson == null) return null
    return JSON.parse(itemJson)
}
  