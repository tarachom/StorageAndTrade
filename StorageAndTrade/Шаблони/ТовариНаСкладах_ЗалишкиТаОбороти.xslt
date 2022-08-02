﻿<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" indent="yes" doctype-system="html" />

	<xsl:template name="Head">
		<meta charset="utf-8" />
		<meta name="viewport" content="width=device-width, initial-scale=1" />
		<link rel="stylesheet" href="Style/bootstrap.min.css" />
		<script src="Style/jquery.slim.min.js"></script>
		<script src="Style/popper.min.js"></script>
		<script src="Style/bootstrap.bundle.min.js"></script>
	</xsl:template>

	<xsl:template match="/root">

		<html>
			<head>
				<title>Table</title>
				<xsl:call-template name="Head" />
			</head>
			<body>

				<div class="container">

					<br/>
					<h2>Залишки та обороти товарів</h2>
					<br/>

					<table class="table table-sm">
						<tr>
							<tr class="table-light">
								<td>
									Період з <xsl:value-of select="head/row/ПочатокПеріоду"/> по <xsl:value-of select="head/row/КінецьПеріоду"/>
								</td>
							</tr>
						</tr>
					</table>

					<table class="table table-bordered table-sm">
						<tr class="table-success">
							<th>Номенклатура</th>
							<th>Характеристика</th>
							<th>Склад</th>
							<th style="text-align:center">На початок</th>
							<th style="text-align:center">Прихід</th>
							<th style="text-align:center">Розхід</th>
							<th style="text-align:center">Оборот</th>
							<th style="text-align:center">На кінець</th>
						</tr>

						<xsl:for-each select="ЗалишкиТаОбороти/row">
							<tr>
								<td>
									<xsl:value-of select="Номенклатура_Назва"/>
								</td>
								<td>
									<xsl:value-of select="Характеристика_Назва"/>
								</td>
								<td>
									<xsl:value-of select="Склад_Назва"/>
								</td>
								<td align="right">
									<xsl:value-of select="ПочатковийЗалишок"/>
								</td>
								<td align="right">
									<xsl:value-of select="Прихід"/>
								</td>
								<td align="right">
									<xsl:value-of select="Розхід"/>
								</td>
								<td align="right">
									<xsl:value-of select="Оборот"/>
								</td>
								<td align="right">
									<xsl:value-of select="КінцевийЗалишок"/>
								</td>
							</tr>
						</xsl:for-each>

					</table>

					<br/>
					<br/>
					<br/>
					<br/>

				</div>

			</body>
		</html>

	</xsl:template>

</xsl:stylesheet>
